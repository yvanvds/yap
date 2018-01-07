using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace YapView
{

  public class YapView : SkiaSharp.Views.WPF.SKElement
  {
    static YapView()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(YapView), new FrameworkPropertyMetadata(typeof(YapView)));
    }

    ObservableCollection<Object> Objects = new ObservableCollection<Object>();
    ObservableCollection<Connection> Connections = new ObservableCollection<Connection>();

    Object currentObj = null;
    Connection currentConn = null;

    Object objectBelowMouse = null;
    Connection connectionBelowMouse = null;

    StringEditor SEdit = new StringEditor();

    bool MouseIsDown = false;
    SKPoint MousePos = new SKPoint();

    DispatcherTimer UpdateCanvas = new DispatcherTimer(DispatcherPriority.Render);

    public YapView()
    {
      UpdateCanvas.Interval = new TimeSpan(0, 0, 0, 0, 50);
      UpdateCanvas.Tick += new EventHandler(UpdateCanvas_Elapsed);
      UpdateCanvas.Start();
    }

    private void UpdateCanvas_Elapsed(object sender, EventArgs e)
    {
      InvalidateVisual();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
      SKSurface surface = e.Surface;
      SKCanvas canvas = surface.Canvas;

      canvas.Clear(SKColors.DimGray);

      foreach(var obj in Objects)
      {
        obj.Draw(canvas);
      }

      foreach(var obj in Connections)
      {
        obj.Draw(canvas);
      }
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      SKPoint pos = new SKPoint();
      pos.X = (float)e.GetPosition(this).X;
      pos.Y = (float)e.GetPosition(this).Y;

      if(objectBelowMouse != null)
      {
        currentObj = objectBelowMouse;
        if (currentObj.Selected)
        {
          if (StartConnection(pos))
          {
            currentObj.Selected = false;
            currentObj = null;
          }
          else
          {
            currentObj.EditMode = true;
            int carretPos = currentObj.FindCarretPos(pos);
            SEdit.Edit(currentObj.Text, carretPos);
            currentObj.CarretPos = carretPos;
          }
        } else
        {
          currentObj.Selected = true;
          int index = Objects.IndexOf(currentObj);
          Objects.Move(index, Objects.Count - 1);
        }
      } else
      {
        if (currentObj != null)
        {
          currentObj.Selected = false;
          currentObj.EditMode = false;
          currentObj = null;
        }
      }

      MouseIsDown = true;
      MousePos = pos;
      e.Handled = true;
    }

    private bool StartConnection(SKPoint pos)
    {
      int pin = currentObj.OnOutput(pos);
      if(pin != -1)
      {
        Connections.Add(new Connection());
        currentConn = Connections.Last();
        currentConn.SetStart(currentObj, pin);
        return true;
      }
      return false;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if(currentConn != null)
      {
        SKPoint pos = new SKPoint();
        pos.X = (float)e.GetPosition(this).X;
        pos.Y = (float)e.GetPosition(this).Y;

        foreach (var obj in Objects)
        {
          if(obj.IsInside(pos))
          {
            int pin = obj.OnInput(pos);
            if(pin == -1 &&obj.Inputs > 0)
            {
              pin = 0;
            }
            if (pin == -1)
            {
              Connections.Remove(currentConn);
              currentConn = null;
              break;
            }
            else
            {
              currentConn.SetEnd(obj, pin);
              currentConn = null;
              break;
            }
          }
        }
        if(currentConn != null)
        {
          Connections.Remove(currentConn);
          currentConn = null;
        }
        //InvalidateVisual();
      }
      MouseIsDown = false;
      e.Handled = true;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      SKPoint pos = new SKPoint();
      pos.X = (float)e.GetPosition(this).X;
      pos.Y = (float)e.GetPosition(this).Y;

      // find object below mouse
      objectBelowMouse = null;
      if(Objects.Count > 0)
      {
        bool HoverFound = false;
        for(int i = Objects.Count - 1; i >= 0; i--)
        {
          if(!HoverFound)
          {
            if (Objects[i].IsInside(pos))
            {
              objectBelowMouse = Objects[i];
              HoverFound = true;
            }
          }
          Objects[i].Hover = false;
        }
      }

      // find connection below mouse
      connectionBelowMouse = null;
      if(Connections.Count > 0)
      {
        Connection nearest = Connections[0];
        float nearestDist = float.MaxValue;
        foreach (var obj in Connections)
        {
          obj.Hover = false;
          if (obj.Complete)
          {
            float dist = obj.DistanceToPos(pos);
            if(dist < nearestDist)
            {
              nearestDist = dist;
              nearest = obj;
            }
          }
        }

        if(nearestDist < 5)
        {
          connectionBelowMouse = nearest;
        }
      }

      // connections get precendence on hover
      if(connectionBelowMouse != null)
      {
        objectBelowMouse = null;
        connectionBelowMouse.Hover = true;
      } else if(objectBelowMouse != null)
      {
        objectBelowMouse.Hover = true;
      }
      

      if(MouseIsDown)
      {
        if(currentObj != null)
        {
          SKPoint delta = (pos - MousePos);
          currentObj.Move(delta);
          //InvalidateVisual();
        } else if (currentConn != null)
        {
          currentConn.SetMousePos(e.GetPosition(this));
          //InvalidateVisual();
        }
        e.Handled = true;
      }
      // important: we also need the mouse position to add new objects
      MousePos = pos;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if(currentObj != null && currentObj.EditMode == true)
      {
        currentObj.Text = SEdit.Update(e.Key);
        currentObj.CarretPos = SEdit.Pos;
        //InvalidateVisual();
      } else
      {
        if(currentObj != null && (e.Key == Key.Delete || e.Key == Key.Back))
        {
          Objects.Remove(currentObj);
          currentObj = null;
          //InvalidateVisual();
        }

        else if (e.Key == Key.O)
        {
          Objects.Add(new Object(MousePos));
          currentObj = Objects.Last();
          currentObj.Inputs = 3;
          currentObj.Outputs = 3;
          currentObj.Selected = true;
          currentObj.EditMode = true;
          SEdit.Edit(currentObj.Text, 0);
          currentObj.CarretPos = 0;
          //InvalidateVisual();
        }
      }
    }
  }
}

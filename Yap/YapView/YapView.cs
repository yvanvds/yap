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
    IYapHandler Handler = null;
    Connections Connections = null;
    Objects Objects = null;

    StringEditor SEdit = new StringEditor();

    bool MouseIsDown = false;
    SKPoint MousePos = new SKPoint();

    DispatcherTimer UpdateCanvas = new DispatcherTimer(DispatcherPriority.Render);

    static YapView()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(YapView), new FrameworkPropertyMetadata(typeof(YapView)));
    }
    
    public void Init(IYapHandler Handler)
    {
      this.Handler = Handler;
      Objects = new Objects(Handler);
      Connections = new Connections(Handler);

      UpdateCanvas.Interval = new TimeSpan(0, 0, 0, 0, 50);
      UpdateCanvas.Tick += new EventHandler(UpdateCanvas_Elapsed);
      UpdateCanvas.Start();
    }

    public void Load(string JSONContent)
    {
      Connections.Clear();
      Objects.Clear();
      Handler.Clear();
      Handler.Load(JSONContent);

      uint num = Handler.NumObjects();

      // load objects
      for(uint i = 0; i < num; i++)
      {
        object obj = Handler.GetObjectFromList(i);
        float x, y;
        Handler.GetPosition(obj, out x, out y);
        Object O = Objects.Add(new SKPoint(x, y), obj);
        O.GetValuesFromHandle();
      }

      // load connections
      foreach(Object O in Objects.List)
      {
        for(uint outlet = 0; outlet < O.Outputs; outlet++)
        {
          uint connections = Handler.GetConnections(O.handle, outlet);
          for(uint j = 0; j < connections; j++)
          {
            uint ObjID = Handler.GetConnectionTarget(O.handle, outlet, j);
            uint inlet = Handler.GetConnectionTargetInlet(O.handle, outlet, j);

            Object I = Objects.GetObjectWithID(ObjID);
            if(I != null)
            {
              Connections.Add(O, outlet, I, inlet);
            }
          }
        }
      }
    }

    public string Save()
    {
      Objects.StorePositions();
      return Handler.Save();
    }

    private bool StartConnection(SKPoint pos)
    {
      int pin = Objects.Current.OnOutput(pos);
      if (pin != -1)
      {
        Connections.Add(Objects.Current, (uint)pin, pos);
        return true;
      }
      return false;
    }

    /*
        DRAW
    */

    private void UpdateCanvas_Elapsed(object sender, EventArgs e)
    {
      InvalidateVisual();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
      SKSurface surface = e.Surface;
      SKCanvas canvas = surface.Canvas;

      canvas.Clear(SKColors.DimGray);

      Objects.Draw(canvas);
      Connections.Draw(canvas);
    }

    /*
        EVENTS
    */
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      SKPoint pos = new SKPoint();
      pos.X = (float)e.GetPosition(this).X;
      pos.Y = (float)e.GetPosition(this).Y;

      Connections.Deselect();

      if (Objects.TrySetCurrent())
      {

        if (Objects.CurrentIsSelected())
        {
          if (StartConnection(pos))
          {
            Objects.Deselect();
          }
          else
          {
            int carret = Objects.EditCurrentOnPos(pos);
            SEdit.Edit(Objects.Current.Text, carret);
          }
        }
        else
        {
          if (StartConnection(pos))
          {
            Objects.Deselect();
          }
          else
          {
            Objects.SelectCurrent();
          }
        }
      }

      else if (Connections.TrySelectBelowMouse())
      {
      }
      else
      {
        if (Objects.Current != null)
        {
          Objects.EndEditMode();

          if (Objects.Current.HasChanged())
          {
            if (Objects.Current.Reconfigure())
            {
              Connections.RemoveConnectedTo(Objects.Current);
            }
          }
          Objects.Deselect();
        }
      }

      MouseIsDown = true;
      MousePos = pos;
      e.Handled = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (Connections.IsBeingCreated())
      {
        SKPoint pos = new SKPoint();
        pos.X = (float)e.GetPosition(this).X;
        pos.Y = (float)e.GetPosition(this).Y;

        foreach (var obj in Objects.List)
        {
          if (obj.IsInside(pos))
          {
            Connections.TrySetCurrentEnd(obj, pos);
            break;
          }
        }
        Connections.DeleteCurrent();
      }
      MouseIsDown = false;
      e.Handled = true;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      SKPoint pos = new SKPoint();
      pos.X = (float)e.GetPosition(this).X;
      pos.Y = (float)e.GetPosition(this).Y;

      // find connection or object below mouse
      if(Connections.SetBelowMouse(pos))
      {
        Objects.BelowMouse = null;
      } else
      {
        Objects.SetBelowMouse(pos);
      }

      // connections get precendence on hover
      if (MouseIsDown)
      {
        if (Objects.Current != null)
        {
          SKPoint delta = (pos - MousePos);
          Objects.Current.Move(delta);
        }
        else if (Connections.IsBeingCreated())
        {
          Connections.Current.SetMousePos(e.GetPosition(this));
        }
        e.Handled = true;
      }
      // important: we also need the mouse position to add new objects
      MousePos = pos;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (Objects.InEditMode())
      {
        string text = SEdit.Update(e.Key);
        Objects.UpdateEditMode(text, SEdit.Pos);
      }

      else if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        if (Objects.Current != null)
        {
          Connections.RemoveConnectedTo(Objects.Current);
          Objects.DeleteCurrent();
        }
        else 
        {
          Connections.DeleteCurrent();
        }
      }

      else if (e.Key == Key.O)
      {
        Objects.Add(MousePos);
        SEdit.Edit(Objects.Current.Text, 0);
      }

    }
  }
}

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

    IYapHandler Handler = null;
    Connections Connections = null;
    Objects Objects = null;

    StringEditor SEdit = new StringEditor();

    bool MouseIsDown = false;
    SKPoint MousePos = new SKPoint();

    DispatcherTimer UpdateCanvas = new DispatcherTimer(DispatcherPriority.Render);

    public void Init(IYapHandler Handler)
    {
      this.Handler = Handler;
      Objects = new Objects(Handler);
      Connections = new Connections(Handler);

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

      Objects.Draw(canvas);
      Connections.Draw(canvas);
    }

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

          if(Objects.Current.HasChanged())
          {
            if(Objects.Current.Reconfigure())
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

    private bool StartConnection(SKPoint pos)
    {
      int pin = Objects.Current.OnOutput(pos);
      if (pin != -1)
      {
        Connections.Add(Objects.Current, pin, pos);
        return true;
      }
      return false;
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

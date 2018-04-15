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
    // The yap view presents objects and connections
    // between objects.
    internal Connections Connections = null;
    internal Widgets Widgets = null;

    bool MouseIsDown = false;

    SKPoint mousePos = new SKPoint();
    public SKPoint MousePos => mousePos;
    WidgetHolder WidgetBelowMouse = null;
    Connection ConnectionBelowMouse = null;

    // Canvas will be updated every 50 milliseconds
    DispatcherTimer UpdateCanvas = new DispatcherTimer(DispatcherPriority.Render);

    Selector Selector;

    IYapHandler handler = null;
    public IYapHandler Handle { get => handler; set => handler = value; }

    static YapView()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(YapView), new FrameworkPropertyMetadata(typeof(YapView)));
    }
    
    public void Init()
    {
      Widgets = new Widgets(this);
      Connections = new Connections(this);
      Selector = new Selector(this);

      UpdateCanvas.Interval = new TimeSpan(0, 0, 0, 0, 50);
      UpdateCanvas.Tick += new EventHandler(UpdateCanvas_Elapsed);
      UpdateCanvas.Start();
    }

    public void Load(string JSONContent)
    {
      Connections.Clear();
      Widgets.Clear();
      Handle.Clear();
      Handle.Load(JSONContent);

      uint num = Handle.NumObjects();

      // load objects
      for(uint i = 0; i < num; i++)
      {
        object obj = Handle.GetObjectFromList(i);
        
        WidgetHolder holder = Widgets.Add(new SKPoint(0, 0), obj);
        holder.Load();
      }

      // load connections
      foreach(WidgetHolder O in Widgets.List)
      {
        for(uint outlet = 0; outlet < O.Widget.Connector.Outputs; outlet++)
        {
          uint connections = Handle.GetConnections(O.Handle, outlet);
          for(uint j = 0; j < connections; j++)
          {
            uint ObjID = Handle.GetConnectionTarget(O.Handle, outlet, j);
            uint inlet = Handle.GetConnectionTargetInlet(O.Handle, outlet, j);

            WidgetHolder I = Widgets.Get(ObjID);
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
      Widgets.Save();
      return Handle.Save();
    }

    /*
        DRAW
    */

    private void UpdateCanvas_Elapsed(object sender, EventArgs e)
    {
      Widgets.Update();
      InvalidateVisual();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
      SKSurface surface = e.Surface;
      SKCanvas canvas = surface.Canvas;

      canvas.Clear(SKColors.DimGray);

      if(Interface.PerformanceMode)
      {
        Connections.Draw(canvas);
        Widgets.Draw(canvas);
      } else
      {
        Widgets.Draw(canvas);
        Connections.Draw(canvas);
      }
      
    }

    private bool TryStartConnection(SKPoint pos)
    {
      if (WidgetBelowMouse == null) return false;
      int pin = WidgetBelowMouse.Widget.Connector.OnOutput(pos);

      if (pin != -1)
      {
        Connections.Add(WidgetBelowMouse, (uint)pin, pos);
        return true;
      }
      return false;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
      base.OnMouseLeave(e);
      MouseIsDown = false;
    }

    /*
        EVENTS
    */
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      MouseIsDown = true;
      e.Handled = true;

      if(WidgetBelowMouse != null)
      {
        int outlet = WidgetBelowMouse.Widget.Connector.OnOutput(mousePos);
        if (!Interface.PerformanceMode && outlet > -1)
        {
          Connections.Add(WidgetBelowMouse, (uint)outlet, mousePos);
        } else
        {
          WidgetBelowMouse.Widget.OnMouseDown(e);
          Selector.Select(WidgetBelowMouse);
        }
      }
      else if(ConnectionBelowMouse != null)
      {
        Selector.Select(ConnectionBelowMouse);
      }
      else
      {
        Selector.CanvasClicked();
      }
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      if (Connections.IsBeingCreated())
      {
        SKPoint pos = new SKPoint
        {
          X = (float)e.GetPosition(this).X,
          Y = (float)e.GetPosition(this).Y
        };

        foreach (var holder in Widgets.List)
        {
          if (holder.Widget.Contains(pos))
          {
            Connections.TrySetCurrentEnd(holder, pos);
            break;
          }
        }
        Connections.DeleteCurrent();
      } else
      {
        Selector.OnMouseLeftButtonUp(e);
      }
      MouseIsDown = false;
      e.Handled = true;
    }



    protected override void OnMouseMove(MouseEventArgs e)
    {
      mousePos.X = (float)e.GetPosition(this).X;
      mousePos.Y = (float)e.GetPosition(this).Y;
      

      if (WidgetBelowMouse != null) WidgetBelowMouse.Widget.BelowMouse = false;
      WidgetBelowMouse = Widgets.At(MousePos);
      if (WidgetBelowMouse != null) WidgetBelowMouse.Widget.BelowMouse = true;

      Connections.SetBelowMouse(MousePos);
      ConnectionBelowMouse = Connections.BelowMouse;

      if (Connections.IsBeingCreated())
      {
        Connections.Current.SetMousePos(e.GetPosition(this));
      }

      if(MouseIsDown) Selector.OnMouseMove(e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      if(WidgetBelowMouse != null)
      {
        WidgetBelowMouse.Widget.OnMouseWheel(e);
      }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      Selector.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
      Selector.OnKeyUp(e);
    }

    // patcher edits
    public void AddObject(bool onMousePos)
    {
      SKPoint pos;
      if (onMousePos) pos = MousePos;
      else pos = new SKPoint(0, 0);
      
      Widgets.Add(pos);
      Selector.Select(Widgets.List.Last());
      Widgets.List.Last().Widget.EnableEditor(pos);
    }

    public void Deselect()
    {
      Selector.Clear();
    }

    public void Clear()
    {
      Connections.Clear();
      Widgets.Clear();
    }
  }
}

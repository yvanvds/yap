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
    // callback interface which will be used to interact
    // with an external object library like YSE
    IYapHandler handler = null;
    public IYapHandler Handler { get => handler; }

    // The yap view presents objects and connections
    // between objects.
    Connections Connections = null;
    Objects Objects = null;

    // Skia does not have input handling on its own.
    // The String Editor enables us to build objects with
    // user text input.
    StringEditor SEdit = new StringEditor();

    bool MouseIsDown = false;
    SKPoint MousePos = new SKPoint();

    // Canvas will be updated every 50 milliseconds
    DispatcherTimer UpdateCanvas = new DispatcherTimer(DispatcherPriority.Render);

    // objects and connections behave differently in
    // edit and performance mode
    private bool performanceMode = false;
    public bool PerformanceMode
    {
      get => performanceMode;
      set
      {
        performanceMode = value;
        Objects.Deselect();
        Connections.Deselect();
      }
    }

    
    static YapView()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(YapView), new FrameworkPropertyMetadata(typeof(YapView)));
    }
    
    public void Init(IYapHandler Handler)
    {
      this.handler = Handler;
      Objects = new Objects(this);
      Connections = new Connections(this);

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
        for(uint outlet = 0; outlet < O.GuiShape.Outputs; outlet++)
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

    /*
        DRAW
    */

    private void UpdateCanvas_Elapsed(object sender, EventArgs e)
    {
      Objects.UpdateGui();
      InvalidateVisual();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
      SKSurface surface = e.Surface;
      SKCanvas canvas = surface.Canvas;

      canvas.Clear(SKColors.DimGray);

      if(PerformanceMode)
      {
        Connections.Draw(canvas);
        Objects.Draw(canvas);
      } else
      {
        Objects.Draw(canvas);
        Connections.Draw(canvas);
      }
      
    }

    private bool TryStartConnection(SKPoint pos)
    {
      if (Objects.Current == null) return false;
      int pin = Objects.Current.GuiShape.OnOutput(pos);
      if (pin != -1)
      {
        Connections.Add(Objects.Current, (uint)pin, pos);
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

      SKPoint pos = new SKPoint();
      pos.X = (float)e.GetPosition(this).X;
      pos.Y = (float)e.GetPosition(this).Y;
      MousePos = pos;

      Connections.Deselect();

      Object previous = Objects.Current;

      if(Objects.TrySetCurrent())
      {
        // clear selected state unless the same item is clicked twice in a row
        if(previous != Objects.Current)
        {
          if(previous != null) previous.Deselect();
          Objects.Current.Deselect();
        }
        // let object handle the click
        Objects.Current.OnMouseLeftButtonDown(pos);
      } else
      {
        if (previous != null)
        {
          Objects.EndEditMode();
          if (previous.HasChanged() && previous.Reconfigure())
          {
            Connections.RemoveConnectedTo(previous);
          }
          Objects.Deselect();
        }
      }

      // connections are only handled in edit mode
      if (PerformanceMode) return;

      // try to create or select a connection
      if(TryStartConnection(pos))
      {
        Objects.Deselect();
      }
      else {
        Connections.TrySelectBelowMouse();
      }
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
          if (obj.GuiShape.IsInside(pos))
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

      if (PerformanceMode)
      {
        if(MouseIsDown)
        {
          if(Objects.Current != null)
          {
            Objects.Current.GuiShape.OnMouseMove(pos);
          }
        } else
        {
          Objects.SetBelowMouse(pos);
        }
        return;
      }

      // else:


      // find connection or object below mouse
      Objects.SetBelowMouse(pos);
      if(Objects.BelowMouse != null)
      {
        if(Objects.BelowMouse.GuiShape.OnOutput(pos) != -1)
        {
          Connections.BelowMouse = null;
        } else
        {
          Connections.SetBelowMouse(pos);
          if(Connections.BelowMouse != null)
          {
            Objects.BelowMouse = null;
          }
        }
      } else
      {
        Connections.SetBelowMouse(pos);
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

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      if(PerformanceMode)
      {
        if(Objects.BelowMouse != null)
        {
          float pos = (float)e.GetPosition(this).X;
          Objects.BelowMouse.OnMouseWheel(e, pos);
        }
      }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if(PerformanceMode)
      {
        if(Objects.Current != null && Objects.Current.GuiShape.GuiEditMode)
        {
          string text = SEdit.Update(e.Key);
          Objects.Current.GuiShape.GuiValue = text;
          Objects.Current.GuiShape.CarretPos = SEdit.Pos;
        }
        return;
      }

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
    }

    // patcher edits
    public void AddObject(bool onMousePos)
    {
      if (onMousePos)
      {
        Objects.Add(MousePos);
      } else
      {
        Objects.Add(new SKPoint(0, 0));
      }
      SEdit.Edit(Objects.Current.GuiShape.Text, 0, EditModeStyle.Line);
    }

    public void Clear()
    {
      Connections.Clear();
      Objects.Clear();
    }
  }
}

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  internal class Widget
  {
    // The corresponding handle to the object
    // in an external library.
    object handle;
    protected object Handle { get => handle; }

    // visible properties
    public ISizer Sizer;

    public bool BelowMouse { get; set; }
    public bool Selected { get; set; }

    protected YapView parent;

    public string Name = "";
    public bool NeedsEvaluation = false;

    public IConnect Connector;
    public IKeyboard Keyboard;

    public Widget(YapView parent, object handle, SKPoint pos)
    {
      this.parent = parent;
      this.handle = handle;

      Sizer = new AutoSizer(pos.X, pos.Y, 80f, 25f);
      Connector = new ConnectDisabled();
      Keyboard = new KeyboardEnabled();
    }

    public bool Contains(SKPoint pos)
    {
      return Sizer.Contains(pos);
    }

    public bool IsInside(SKRect rect)
    {
      return rect.Contains(Sizer.Rect);
    }

    public void EnableEditor(SKPoint pos)
    {
      Keyboard.Focus = true;
      Keyboard.Init(Name, pos, Sizer.Rect, Paint.Text);
      Keyboard.Edit(EditModeStyle.Line);
    }

    public void DisableEditor()
    {
      Keyboard.Focus = false;
    }

    public void Deselect()
    {
      Selected = false;
      if(Keyboard.Focus)
      {
        Name = Keyboard.Text;
        NeedsEvaluation = true;
        Keyboard.Focus = false;
      }
    }

    void EvaluateWidth()
    {
      if(Sizer.Mode == SizeMode.AUTO)
      {
        if(Keyboard.Focus)
        {
          Sizer.Resize(Keyboard.Text, Paint.Text);
        } else
        {
          Sizer.Resize(Name, Paint.Text);
        }
      }
    }

    public virtual void OnMouseDown(MouseButtonEventArgs e)
    {
      // first click selects
      if (!Interface.PerformanceMode && !Selected)
      {
        Selected = true;
        return;
      }

      if(!Interface.PerformanceMode && Keyboard.AcceptsInput())
      {
        SKPoint pos = new SKPoint
        {
          X = (float)e.GetPosition(parent).X,
          Y = (float)e.GetPosition(parent).Y,
        };
        EnableEditor(pos);
      }
    }

    public virtual void OnMouseUp(MouseButtonEventArgs e)
    {
      Sizer.OnMouseUp();
    }

    public virtual void OnMouseMove(MouseEventArgs e)
    {
      if (Interface.PerformanceMode) return;
      
      SKPoint pos = new SKPoint
      {
        X = (float)e.GetPosition(parent).X - Sizer.Rect.Left,
        Y = (float)e.GetPosition(parent).Y - Sizer.Rect.Top,
      };

      DisableEditor();
      Sizer.Move(pos);

      Connector.Update(Sizer.Rect);
    }

    public void Move(SKPoint delta)
    {
      if (Interface.PerformanceMode) return;
      DisableEditor();
      Sizer.Move(delta);
      Connector.Update(Sizer.Rect);
    }

    public virtual void OnMouseWheel(MouseWheelEventArgs e)
    {

    }

    public virtual void Update()
    {
      if(Selected)
      {
        EvaluateWidth();
      }
    }

    public virtual void Draw(SKCanvas canvas)
    {
      canvas.DrawRect(Sizer.Rect, Paint.ObjectFill);
      
      if(!Interface.PerformanceMode) Connector.Draw(Sizer.Rect, canvas);

      if(Keyboard.Focus)
      {
        Keyboard.Draw(Sizer.Rect, canvas);
      } else
      {
        DrawGui(canvas);
      }

      if (Selected)
      {
        canvas.DrawRect(Sizer.Rect, Paint.ObjectBorderSelected);
        Sizer.Draw(canvas);
      }
      else if (BelowMouse)
      {
        canvas.DrawRect(Sizer.Rect, Paint.ObjectBorderHovered);
      }
    }

    public virtual void DrawGui(SKCanvas canvas)
    {
      canvas.DrawText(Name, Sizer.Rect.Left + 4, Sizer.Rect.Top + 17, Paint.Text);
    }

    public virtual void Load()
    {
      if (handle == null) return;

      string width = parent.Handle.GetGuiProperty(handle, Properties.WIDTH);
      string height = parent.Handle.GetGuiProperty(handle, Properties.HEIGHT);
      if(width.Length > 0 && height.Length > 0)
      {
        Sizer.SetSize(Convert.ToFloat(width), Convert.ToFloat(height));
      }

      string left = parent.Handle.GetGuiProperty(handle, Properties.POSX);
      if(left.Length > 0)
      {
        Sizer.SetLeft(Convert.ToFloat(left));
      }
      
      string top = parent.Handle.GetGuiProperty(handle, Properties.POSY);
      if(top.Length > 0)
      {
        Sizer.SetTop(Convert.ToFloat(top));
      }

      EvaluateWidth();

      Connector.Inputs = (uint)parent.Handle.GetObjectInputCount(handle);
      Connector.Outputs = (uint)parent.Handle.GetObjectOutputCount(handle);
      Connector.Update(Sizer.Rect);
    }

    public virtual void Save()
    {
      if (handle == null) return;
      parent.Handle.SetGuiProperty(handle, Properties.POSX, Sizer.Rect.Left.ToString(CultureInfo.InvariantCulture));
      parent.Handle.SetGuiProperty(handle, Properties.POSY, Sizer.Rect.Top.ToString(CultureInfo.InvariantCulture));

      if (Sizer.Mode == SizeMode.SQUARE || Sizer.Mode == SizeMode.FLEX)
      {
        parent.Handle.SetGuiProperty(handle, Properties.WIDTH, Sizer.Rect.Width.ToString(CultureInfo.InvariantCulture));
        parent.Handle.SetGuiProperty(handle, Properties.HEIGHT, Sizer.Rect.Height.ToString(CultureInfo.InvariantCulture));
      }
    }

    public void Release()
    {
      if (handle != null)
      {
        parent.Handle.DeleteObject(handle);
      }
      handle = null;
    }
  }
}

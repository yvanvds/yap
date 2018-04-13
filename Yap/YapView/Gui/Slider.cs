using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class Slider : Widget
  {
    float sliderPos = 0;

    public Slider(YapView parent, object handle, SKPoint pos)
      : base(parent, handle, pos)
    {
      Sizer = new FlexSizer(pos.X, pos.Y, 150f, 25f);
      Connector = new ConnectEnabled();
    }

    public override void Update()
    {
      base.Update();
      string guiValue = Interface.Handle.GetGuiValue(Handle);
      sliderPos = Convert.ToFloat(guiValue);
    }

    public override void DrawGui(SKCanvas canvas)
    {
      float width = Sizer.Rect.Size.Width - 2;
      width *= sliderPos;
      if (width < 10)
      {
        width = 10;
      }

      SKRect slider = new SKRect(Sizer.Rect.Left + 1, Sizer.Rect.Top + 5, Sizer.Rect.Left + 1 + width, Sizer.Rect.Bottom - 5);
      if (sliderPos == 0)
      {
        canvas.DrawRect(slider, Paint.SliderZero);
      }
      else
      {
        canvas.DrawRect(slider, Paint.SliderActive);
      }

      if (width > 10)
      {
        canvas.DrawLine(Sizer.Rect.Left + width - 10, Sizer.Rect.Top + 5, Sizer.Rect.Left + width - 10, Sizer.Rect.Bottom - 5, Paint.SliderHandle);
      }
    }

    public override void OnMouseWheel(MouseWheelEventArgs e)
    {
      base.OnMouseWheel(e);
      if (!Interface.PerformanceMode) return;
      
      float delta = e.Delta > 0 ? 0.05f : -0.05f;
      sliderPos += delta;

      Interface.Handle.SendFloatData(Handle, sliderPos);
    }

    public override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      if (!Interface.PerformanceMode) return;

      float pos = parent.MousePos.X - Sizer.Rect.Left;
      pos /= Sizer.Rect.Width;
      sliderPos = pos;
      Interface.Handle.SendFloatData(Handle, sliderPos);
    }

    public override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (!Interface.PerformanceMode) return;

      float pos = parent.MousePos.X - Sizer.Rect.Left;
      pos /= Sizer.Rect.Width;
      sliderPos = pos;
      Interface.Handle.SendFloatData(Handle, sliderPos);
    }
  }
}

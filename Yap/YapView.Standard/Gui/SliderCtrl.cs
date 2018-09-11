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
  class SliderCtrl : GuiObject
  {
    float sliderPos = 0;

    public SliderCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SKSize(150f, 25f);
      rect.Size = size;
      hasEditableGui = false;
      hasFixedSize = true;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.SliderBackground);

      DrawPins(canvas);

      float width = rect.Size.Width - 2;
      width *= sliderPos;
      if(width < 10)
      {
        width = 10;
      }

      SKRect slider = new SKRect(rect.Left + 1, rect.Top + 5, rect.Left + 1 + width, rect.Bottom -5);
      if(sliderPos == 0)
      {
        canvas.DrawRect(slider, Paint.SliderZero);
      } else
      {
        canvas.DrawRect(slider, Paint.SliderActive);
      }
      
      if(width > 10)
      {
        canvas.DrawLine(rect.Left + width - 10, rect.Top + 5, rect.Left + width - 10, rect.Bottom - 5, Paint.SliderHandle);
      }
    }

    public override void UpdateGui()
    {
      guiValue = obj.GetGuiValue();
      sliderPos = float.Parse(guiValue, CultureInfo.InvariantCulture.NumberFormat);
    }

    public override void OnMouseWheel(MouseWheelEventArgs e, float posX)
    {
      float delta = e.Delta > 0 ? 0.05f : -0.05f;
      sliderPos += delta;

      Interface.Handle.SendFloatData(obj.handle, sliderPos);
    }

    public override bool OnMouseDown(SKPoint mousePos)
    {
      if (!base.OnMouseDown(mousePos))
      {
        float pos = mousePos.X - rect.Left;
        pos /= rect.Width;
        sliderPos = pos;
        Interface.Handle.SendFloatData(obj.handle, sliderPos);
      }
      return true;
    }

    public override void OnMouseMove(SKPoint mousePos)
    {
      float pos = mousePos.X - rect.Left;
      pos /= rect.Width;
      sliderPos = pos;
      Interface.Handle.SendFloatData(obj.handle, sliderPos);
    }
  }
}

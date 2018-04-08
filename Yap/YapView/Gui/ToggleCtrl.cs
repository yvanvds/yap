using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class ToggleCtrl : Base
  {
    bool on = false;

    public ToggleCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SkiaSharp.SKSize(25f, 25f);
      rect.Size = size;
      hasEditableGui = false;
      hasFixedSize = true;
      font = Paint.Text;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.ButtonBackground);

      DrawPins(canvas);

      SKPaint paint = Paint.ToggleOff;
      if (on)
      {
        paint = Paint.ToggleOn;
        
      }
      canvas.DrawLine(rect.Left + 7, rect.Bottom - 7, rect.Right - 7, rect.Top + 7, paint);
      canvas.DrawLine(rect.Left + 7, rect.Top + 7, rect.Right - 7, rect.Bottom - 7, paint);
    }

    public override void UpdateGui()
    {
      guiValue = obj.GetGuiValue();
      if(guiValue == "on")
      {
        on = true;
      } else
      {
        on = false;
      }
    }

    public override bool OnMouseDown(SKPoint mousePos)
    {
      if (!base.OnMouseDown(mousePos))
      {
        // this is reversed because the update gui method
        // will do the actual update of on
        obj.View.Handler.SendIntData(obj.handle, on ? 0 : 1);
      }
      return true;
    }
  }
}

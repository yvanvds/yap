using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class CounterCtrl : GuiObject
  {
    public CounterCtrl(SKPoint pos, Object obj) : base(pos,obj)
    {
      size = new SkiaSharp.SKSize(80f, 30f);
      rect.Size = size;
      hasEditableGui = false;
      hasFixedSize = true;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.ObjectFill);

      DrawPins(canvas);

      canvas.DrawText(guiValue, rect.Left + 4, rect.Top + 20, font);
    }

    public override void UpdateGui()
    {
      guiValue = obj.GetGuiValue();
    }
  }
}

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class TextCtrl : GuiObject
  {
    public TextCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SkiaSharp.SKSize(80f, 25f);
      rect.Size = size;

      hasEditableGui = true;
      hasFixedSize = false;
      font = Paint.Message;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      if(!Interface.PerformanceMode)
      {
        canvas.DrawRect(rect, Paint.ObjectBorder);
      }

      canvas.DrawText("test", rect.Left + 4, rect.Top + 17, font);
    }

    public override void UpdateGui()
    {
      
    }
  }
}

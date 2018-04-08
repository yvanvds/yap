using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace YapView.Gui
{
  class BasicCtrl : Base
  {
    

    public BasicCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SKSize(100f, 25f);
      rect.Size = size;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      if(Selected)
      {
        canvas.DrawRect(rect, Paint.ObjectFillSelected);
      } else
      {
        canvas.DrawRect(rect, Paint.ObjectFill);
      }


      DrawPins(canvas);

      canvas.DrawText(Text, rect.Left + 4, rect.Top + 17, Paint.Text);

      if(Hover)
      {
        canvas.DrawRect(rect, Paint.ObjectBorderHovered);
      } else if (Selected)
      {
        canvas.DrawRect(rect, Paint.ObjectBorderSelected);
      }
    }

    public override void UpdateGui()
    {
      
    }
  }
}

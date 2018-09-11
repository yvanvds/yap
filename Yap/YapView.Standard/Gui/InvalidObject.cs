using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  internal class InvalidObject : Widget
  {
    public InvalidObject(YapView parent, object handle, SKPoint pos)
      :base(parent, handle, pos)
    {

    }

    public override void Draw(SKCanvas canvas)
    {
      canvas.DrawRect(Sizer.Rect, Paint.InvalidBackground);

      if (Keyboard.Focus)
      {
        Keyboard.Draw(Sizer.Rect, canvas);
      }
      else
      {
        DrawGui(canvas);
      }

      if (Selected)
      {
        canvas.DrawRect(Sizer.Rect, Paint.ObjectBorderSelected);
      }
      else if (BelowMouse)
      {
        canvas.DrawRect(Sizer.Rect, Paint.ObjectBorderHovered);
      }
    }
  }

}

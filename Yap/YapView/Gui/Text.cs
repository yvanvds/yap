using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class Text : Widget
  {
    string[] guiValue;

    public Text(YapView parent, object handle, SKPoint pos)
      : base(parent, handle, pos)
    {
      Sizer = new FlexSizer(pos.X, pos.Y, 150f, 25f);
      Connector = new ConnectDisabled();
    }

    public override void Update()
    {
      base.Update();
      guiValue = Name.Split(' ');
    }

    public override void Draw(SKCanvas canvas)
    {
      if(!Interface.PerformanceMode)
      {
        canvas.DrawRect(Sizer.Rect, Paint.TextRect);
      }

      if(Keyboard.Focus)
      {
        Keyboard.Draw(Sizer.Rect, canvas);
      } else
      {
        DrawGui(canvas);
      }

      if (Interface.PerformanceMode) return;

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

    public override void DrawGui(SKCanvas canvas)
    {
      float posX = Sizer.Rect.Left + 4;
      float posY = Sizer.Rect.Top + 12;
      float width = Sizer.Rect.Size.Width - 8;
      float height = Sizer.Rect.Size.Height;
      int stringPos = 1;
      String line = "";

      while (stringPos < guiValue.Length)
      {
        if(Paint.Text.MeasureText(line + " " + guiValue[stringPos]) < width)
        {
          line += " " + guiValue[stringPos];
          stringPos++;
        } else
        {
          canvas.DrawText(line, posX, posY, Paint.Text);
          posY += 15;
          line = guiValue[stringPos];
          stringPos++;

          if (posY - Sizer.Rect.Top > height) return;
        }
      }
      canvas.DrawText(line, posX, posY, Paint.Text);

    }
  }
}

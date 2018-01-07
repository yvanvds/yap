using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YapView
{
  public class Object
  {
    SKPaint objectPaint = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = SKColors.AntiqueWhite,
      
    };

    SKPaint objectBorderPaint = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Black,
    };

    SKPaint objectBorderSelectedPaint = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Green,
    };

    SKPaint greenPaint = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      Color = SKColors.Green
    };

    SKPaint textPaint = new SKPaint()
    {
      Color = SKColors.Black,
      Typeface = SKTypeface.FromFamilyName("consolas"),
      TextSize = 12
    };

    SKRect rect;
    SKSize size;

    public Object(float left, float top)
    {
      rect = new SKRect();
      size = new SKSize(100f, 20f);
      
      rect.Left = left;
      rect.Top = top;
      rect.Size = size;

    }

    String text = "";
    public String Text
    {
      get => text;
      set
      {
        text = value;
        float width = 8 + textPaint.MeasureText(Text);
        if (width < 80) width = 80;
        Resize(width, size.Height);
      }
    }

    public bool Selected { get; set; }
    public bool EditMode { get; set; }

    public bool IsInside(Point pos)
    {
      if(pos.X > rect.Left && pos.X < rect.Left + rect.Width)
      {
        if(pos.Y > rect.Top && pos.Y < rect.Top + rect.Height)
        {
          return true;
        }
      }
      return false;
    }
    

    public void Draw(SKCanvas canvas)
    {
      canvas.DrawRect(rect, objectPaint);
      if(Selected)
      {
        canvas.DrawRect(rect, objectBorderSelectedPaint);
      } else
      {
        canvas.DrawRect(rect, objectBorderPaint);
      }

      canvas.DrawLine(rect.Left, rect.Top + 3, rect.Right, rect.Top + 3, objectBorderPaint);
      canvas.DrawLine(rect.Left, rect.Bottom - 3, rect.Right, rect.Bottom - 3, objectBorderPaint);

      canvas.DrawText(Text, rect.Left + 4, rect.Top + 15, textPaint);
    }

    private void Resize(float width, float height)
    {
      size.Width = width;
      size.Height = height;
      rect.Size = size;
    }
  }
}

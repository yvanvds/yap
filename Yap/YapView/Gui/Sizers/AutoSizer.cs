using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace YapView.Gui
{
  internal class AutoSizer : ISizer
  {
    SKSize size = new SKSize();

    SKRect rect = new SKRect();
    public SKRect Rect => rect;

    SizeMode mode = SizeMode.AUTO;
    public SizeMode Mode => mode;

    float minWidth = 80f;
    public float MinWidth { get => minWidth; set => minWidth = value; }

    float minHeight = 25f;
    public float MinHeight { get => minHeight; set => minHeight = value; }

    bool show = false;
    public bool Show { get => show; set => show = value; }

    public AutoSizer(float left, float top, float width, float height)
    {
      rect.Left = left;
      rect.Top = top;
      size.Width = width;
      size.Height = height;
      rect.Size = size;
    }

    public void Draw(SKCanvas canvas)
    {
      // nothing to draw
    }

    public void Move(SKPoint offset)
    {
      rect.Left += offset.X;
      rect.Top += offset.Y;
      rect.Size = size;
    }


    public bool Contains(SKPoint pos)
    {
      return rect.Contains(pos);
    }

    public bool OnHandle(SKPoint pos)
    {
      return false;
    }

    public void Resize(string text, SKPaint paint)
    {
      float width = paint.MeasureText(text) + 8;
      if (width < minWidth) width = minWidth;
      size.Width = width;
      rect.Size = size;
    }

    public void SetLeft(float value)
    {
      rect.Left = value;
      rect.Size = size;
    }

    public void SetTop(float value)
    {
      rect.Top = value;
      rect.Size = size;
    }

    public void OnMouseUp()
    {
      
    }

    public void SetSize(float width, float height)
    {
      
    }
  }
}

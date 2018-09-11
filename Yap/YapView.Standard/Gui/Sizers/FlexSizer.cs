using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class FlexSizer : ISizer
  {
    SKSize size = new SKSize();
    SKPoint handle = new SKPoint();

    SKRect rect = new SKRect();
    public SKRect Rect => rect;

    bool onHandle = false;
    bool resizing = false;

    SizeMode mode = SizeMode.SQUARE;
    public SizeMode Mode => mode;

    float minWidth = 25f;
    public float MinWidth { get => minWidth; set => minWidth = value; }

    float minHeight = 25f;
    public float MinHeight { get => minHeight; set => minHeight = value; }

    bool show = false;
    public bool Show { get => show; set => show = value; }

    public FlexSizer(float left, float top, float width, float height)
    {
      rect.Left = left;
      rect.Top = top;
      size.Width = width;
      size.Height = height;
      rect.Size = size;
      handle.X = rect.Right;
      handle.Y = rect.Bottom;
    }

    public void Draw(SKCanvas canvas)
    {
      if (Interface.PerformanceMode) return;
      canvas.DrawCircle(handle.X, handle.Y, 5, onHandle ? Paint.HandlerActiveBackground : Paint.HandlerBackground);
      canvas.DrawCircle(handle.X, handle.Y, 5, Paint.HandlerBorder);
    }

    public void Move(SKPoint offset)
    {
      if (onHandle || resizing)
      {
        resizing = true;

        size.Width = offset.X;
        size.Height = offset.Y;

        if (size.Width < MinWidth)
        {
          size.Width = MinWidth;
        }
        if (size.Height < MinHeight)
        {
          size.Height = MinHeight;
        }

      }
      else
      {
        rect.Left += offset.X;
        rect.Top += offset.Y;
      }

      rect.Size = size;
      handle.X = rect.Right;
      handle.Y = rect.Bottom;
    }

    public bool Contains(SKPoint pos)
    {
      if (OnHandle(pos))
      {
        onHandle = true;
        return true;
      }
      else
      {
        onHandle = false;
        return rect.Contains(pos);
      }
    }

    public bool OnHandle(SKPoint pos)
    {
      return (MathTools.FindDistance(pos, handle) < 6);
    }

    public void Resize(string text, SKPaint paint)
    {

    }

    public void SetLeft(float value)
    {
      rect.Left = value;
      rect.Size = size;
      handle.X = rect.Right;
      handle.Y = rect.Bottom;
    }

    public void SetTop(float value)
    {
      rect.Top = value;
      rect.Size = size;
      handle.X = rect.Right;
      handle.Y = rect.Bottom;
    }

    public void OnMouseUp()
    {
      resizing = false;
    }

    public void SetSize(float width, float height)
    {
      size.Width = width;
      size.Height = height;
    }
  }
}


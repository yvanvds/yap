using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class Widget
  {
    // The corresponding handle to the object
    // in an external library.
    object handle;
    protected object Handle { get; }

    // visible properties
    protected SKRect rectangle;
    public SKRect Rectangle { get => rectangle; }

    protected SKSize size;
    public SKSize Size { get => size; }

    public bool BelowMouse { get; set; }
    public bool Selected { get; set; }

    YapView parent;

    public Widget(YapView parent, object handle, SKPoint pos)
    {
      this.parent = parent;
      this.handle = handle;

      size = new SKSize
      {
        Width = 80f,
        Height = 25f
      };

      rectangle = new SKRect
      {
        Left = pos.X,
        Top = pos.Y,
        Size = size
      };
      
    }

    public bool Contains(SKPoint pos)
    {
      return rectangle.Contains(pos);
    }

    public virtual void OnMouseDown(MouseButtonEventArgs e)
    {
      
    }

    public virtual void OnMouseUp(MouseButtonEventArgs e)
    {

    }

    public virtual void OnMouseMove(MouseEventArgs e)
    {
      SKPoint pos = new SKPoint
      {
        X = (float)e.GetPosition(parent).X,
        Y = (float)e.GetPosition(parent).Y
      };

      rectangle.Left += pos.X;
      rectangle.Top += pos.Y;
    }

    public virtual void OnMouseWheel(MouseWheelEventArgs e)
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Draw(SKCanvas canvas)
    {
      canvas.DrawRect(rectangle, Paint.ObjectBorder);
    }
  }
}

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class Toggle : Widget
  {
    bool on = false;

    public Toggle(YapView parent, object handle, SKPoint pos)
      :base(parent, handle, pos)
    {
      Sizer = new SquareSizer(pos.X, pos.Y, 25f, 25f);
      Connector = new ConnectEnabled();
    }

    public override void Update()
    {
      base.Update();
      string value = parent.Handle.GetGuiValue(Handle);
      if (value == "on")
      {
        on = true;
      }
      else on = false;
    }

    public override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      if (!Interface.PerformanceMode) return;

      parent.Handle.SendIntData(Handle, on ? 0 : 1);
    }

    public override void DrawGui(SKCanvas canvas)
    {
      SKPaint paint = on? Paint.ToggleOn : Paint.ToggleOff;

      float stroke = Sizer.Rect.Size.Width * 0.1f;
      if(stroke < 2f)
      {
        stroke = 2f;
      }
      paint.StrokeWidth = stroke;
      float space = stroke * 3;

      canvas.DrawLine(Sizer.Rect.Left + space, Sizer.Rect.Bottom - space, Sizer.Rect.Right - space, Sizer.Rect.Top + space, paint);
      canvas.DrawLine(Sizer.Rect.Left + space, Sizer.Rect.Top + space, Sizer.Rect.Right - space, Sizer.Rect.Bottom - space, paint);
    }
  }
}

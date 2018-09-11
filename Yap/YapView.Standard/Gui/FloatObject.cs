using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class FloatObject : Widget
  {
    string guiValue = "";

    public FloatObject(YapView parent, object handle, SKPoint pos)
      : base(parent, handle, pos)
    {
      Sizer = new AutoSizer(pos.X, pos.Y, 80f, 25f);
      Connector = new ConnectEnabled();
    }

    public override void Update()
    {
      base.Update();
      guiValue = parent.Handle.GetGuiValue(Handle);

    }

    public override void DrawGui(SKCanvas canvas)
    {
      SKPoint pos = new SKPoint
      {
        X = Sizer.Rect.Left + 4,
        Y = Sizer.Rect.Top + 18
      };
      canvas.DrawText(guiValue, pos.X, pos.Y, Paint.TextInt);
    }

    public override void OnMouseWheel(MouseWheelEventArgs e)
    {
      float value = Convert.ToFloat(guiValue);
      float delta = e.Delta > 0 ? 1 : -1;

      float multiplier = 1;
      float pos = parent.MousePos.X - Sizer.Rect.Left;

      while (pos > 30)
      {
        multiplier /= 10;
        pos -= 5;
      }

      value += (delta * multiplier);
      parent.Handle.SendFloatData(Handle, value);
    }

    public override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);

      if (Interface.PerformanceMode)
      {
        parent.Handle.SendBang(Handle);
      }
    }
  }
}

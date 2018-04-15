using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  internal class Button : Widget
  {
    int blink = 0;

    public Button(YapView parent, object handle, SKPoint pos)
      : base(parent, handle, pos)
    {
      Sizer = new SquareSizer(pos.X, pos.Y, 25f, 25f);
      Connector = new ConnectEnabled();
    }

    public override void Update()
    {
      base.Update();
      string value = parent.Handle.GetGuiValue(Handle);
      if(value == "on")
      {
        blink = 2;
      } else if(blink > 0)
      {
        blink--;
      }
    }

    public override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);

      if(Interface.PerformanceMode)
      {
        parent.Handle.SendBang(Handle);
      }
    }

    public override void DrawGui(SKCanvas canvas)
    {
      float size = Sizer.Rect.Height * 0.3f;
      float stroke = size * 0.2f;
      if (stroke < 2f) stroke = 2f;
      Paint.ButtonCircle.StrokeWidth = stroke; 
      canvas.DrawCircle(Sizer.Rect.MidX, Sizer.Rect.MidY, size, Paint.ButtonCircle);

      if(blink > 0)
      {
        canvas.DrawCircle(Sizer.Rect.MidX, Sizer.Rect.MidY, size * 0.9f, Paint.ButtonBlink);
      }
    }
  }
}

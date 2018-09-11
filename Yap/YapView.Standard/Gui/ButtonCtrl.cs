using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace YapView.Gui
{
  class ButtonCtrl : GuiObject
  {
    int blink = 0;

    public ButtonCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SkiaSharp.SKSize(25f, 25f);
      rect.Size = size;
      hasEditableGui = false;
      hasFixedSize = true;
      font = Paint.Text;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.ButtonBackground);

      DrawPins(canvas);
      canvas.DrawCircle(rect.MidX, rect.MidY, 7, Paint.ButtonCircle);

      if(blink > 0)
      {
        canvas.DrawCircle(rect.MidX, rect.MidY, 4, Paint.ButtonBlink);
        blink--;
      }

      //canvas.DrawText("Push", buttonBack.Left + 6, buttonBack.Top + 16, font);
    }

    public override void UpdateGui()
    {
      guiValue = obj.GetGuiValue();
      if(guiValue == "on")
      {
        blink = 2;
      } 
    }

    public override bool OnMouseDown(SKPoint mousePos)
    {
      if(!base.OnMouseDown(mousePos))
      {
        Interface.Handle.SendBang(obj.handle);
      }
      return true;
    }
  }
}

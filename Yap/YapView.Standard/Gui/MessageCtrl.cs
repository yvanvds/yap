using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class MessageCtrl : GuiObject
  {
    int highlight = 0;

    public MessageCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SkiaSharp.SKSize(80f, 25f);
      rect.Size = size;
      hasEditableGui = false;
      hasFixedSize = false;
      font = Paint.Message;
    }

    public override void EvaluateArguments(string args)
    {
      Interface.Handle.SendStringData(obj.handle, args);
      guiValue = args;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.ButtonBackground);
      if (Interface.PerformanceMode)
      {
        if(highlight > 0)
        {
          canvas.DrawRect(rect, Paint.MessageBorderBlink);
        } else
        {
          canvas.DrawRect(rect, Paint.MessageBorder);
        }
        
      }
      if (highlight > 0) highlight--;

      DrawPins(canvas);

      canvas.DrawText(guiValue, rect.Left + 4, rect.Top + 17, font);
    }

    public override bool OnMouseDown(SKPoint mousePos)
    {
      if(!base.OnMouseDown(mousePos))
      {
        Interface.Handle.SendBang(obj.handle);
        highlight = 2;
      }
      return true;
    }

    public override void UpdateGui()
    {
      
    }
  }
}

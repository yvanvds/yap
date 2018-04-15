using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class Message : Widget
  {
    int highlight = 0;
    string guiValue = "";

    public Message(YapView parent, object handle, SKPoint pos) 
      :base(parent, handle, pos)
    {
      Sizer = new AutoSizer(pos.X, pos.Y, 80f, 25f);
      Connector = new ConnectEnabled();
    }

    public override void Update()
    {
      base.Update();
      string[] args = Name.Split(new[] { ' ' }, 2);
      if(args.Length > 1)
      {
        guiValue = args[1];
      } else
      {
        guiValue = "message";
      }
    }

    public override void DrawGui(SKCanvas canvas)
    {
      if (Interface.PerformanceMode)
      {
        if (highlight > 0)
        {
          canvas.DrawRect(Sizer.Rect, Paint.MessageBorderBlink);
        }
        else
        {
          canvas.DrawRect(Sizer.Rect, Paint.MessageBorder);
        }

      }
      if (highlight > 0) highlight--;

      canvas.DrawText(guiValue, Sizer.Rect.Left + 4, Sizer.Rect.Top + 17, Paint.Message);
    }

    public override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      if(Interface.PerformanceMode)
      {
        parent.Handle.SendBang(Handle);
        highlight = 2;
      }
    }
  }
}

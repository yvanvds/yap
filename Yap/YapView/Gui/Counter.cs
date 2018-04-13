using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class Counter : Widget
  {
    string guiValue = "";

    public Counter(YapView parent, object handle, SKPoint pos)
      : base(parent, handle, pos)
    {
      Sizer = new LockedSizer(pos.X, pos.Y, 60f, 25f);
      Connector = new ConnectEnabled();
    }

    public override void Update()
    {
      base.Update();
      guiValue = Interface.Handle.GetGuiValue(Handle);

    }

    public override void DrawGui(SKCanvas canvas)
    {
      SKPoint pos = new SKPoint
      {
        X = Sizer.Rect.Left + 4,
        Y = Sizer.Rect.Top + 18
      };
      canvas.DrawText(guiValue, pos.X, pos.Y, Paint.Text);
    }
  }
}

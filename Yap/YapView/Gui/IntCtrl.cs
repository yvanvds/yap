using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class IntCtrl : Base
  {
    int previousMousePos;

    public IntCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SkiaSharp.SKSize(100f, 30f);
      rect.Size = size;
      previousMousePos = (int)pos.Y;
      hasEditableGui = true;
      hasFixedSize = true;
      font = Paint.Text;
      GuiEditModeStyle = EditModeStyle.Int;
    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.IntBackground);
      canvas.DrawRect(rect, Paint.ObjectBorder);

      DrawPins(canvas);

      canvas.DrawText(guiValue, rect.Left + 4, rect.Top + 20, font);
    }

    public override void DrawGuiEdit(SKCanvas canvas)
    {
      canvas.DrawText(guiValue, rect.Left + 4, rect.Top + 20, Paint.TextIntCtrl);
      string s = guiValue;
      if (CarretPos < guiValue.Length) s = s.Remove(CarretPos);
      float xPos = rect.Left + 4 + font.MeasureText(s);
      canvas.DrawLine(xPos, rect.Top + 5, xPos, rect.Bottom - 4, Paint.Carret);
    }

    public override void UpdateGui()
    {
      guiValue = obj.GetGuiValue();
    }

    public override void OnMouseWheel(MouseWheelEventArgs e, float posX)
    {
      int value = Int32.Parse(guiValue);
      float delta = e.Delta > 0 ? 1 : -1;

      // steps are bigger when on the right of the control
      float multiplier = 1;
      posX -= rect.Left;
      while(posX > 30)
      {
        multiplier++;
        posX -= 5;
      }
      value += (int)(delta * multiplier);
      obj.View.Handler.SendIntData(obj.handle, value);
    }

    public override bool OnMouseDown(SKPoint mousePos)
    {
      if(!base.OnMouseDown(mousePos))
      {
        previousMousePos = (int)mousePos.Y;
      }
      return true;
    }

    public override void OnMouseMove(SKPoint mousePos)
    {
      int value = Int32.Parse(guiValue);
      int add = (int)((previousMousePos - mousePos.Y));
      
      obj.View.Handler.SendIntData(obj.handle, value + add);
      previousMousePos = (int)mousePos.Y;
    }

    protected override void SendGuiValue(string value)
    {
      try
      {
        int i = Int32.Parse(value);
        obj.View.Handler.SendIntData(obj.handle, i);
      } catch (FormatException)
      {

      }
      
    }
  }
}

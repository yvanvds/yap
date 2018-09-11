using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  class FloatCtrl : GuiObject
  {

    public FloatCtrl(SKPoint pos, Object obj) : base(pos, obj)
    {
      size = new SkiaSharp.SKSize(100f, 30f);
      rect.Size = size;
      hasEditableGui = true;
      hasFixedSize = true;
      font = Paint.Text;
      GuiEditModeStyle = EditModeStyle.Float;

    }

    public override void DrawGui(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.FloatBackground);

      DrawPins(canvas);

      canvas.DrawText(guiValue, rect.Left + 4, rect.Top + 20, font);
    }

    public override void DrawGuiEdit(SKCanvas canvas)
    {
      canvas.DrawText(guiValue, rect.Left + 4, rect.Top + 20, Paint.TextFloatCtrl);
      string s = guiValue;
      if (CarretPos < guiValue.Length) s = s.Remove(CarretPos);
      float xPos = rect.Left + 4 + font.MeasureText(s);
      canvas.DrawLine(xPos, rect.Top + 5, xPos, rect.Bottom - 4, Paint.Carret);
    }

    public override void UpdateGui()
    {
      guiValue = obj.GetGuiValue();
      float value = float.Parse(guiValue, CultureInfo.InvariantCulture.NumberFormat);
      guiValue = value.ToString("0.000", CultureInfo.InvariantCulture.NumberFormat);
    }

    public override void OnMouseWheel(MouseWheelEventArgs e, float posX)
    {
      float value = float.Parse(guiValue, CultureInfo.InvariantCulture.NumberFormat);
      float delta = e.Delta > 0 ? 1 : -1;

      // steps are bigger when on the right of the control
      float multiplier = 1;
      posX -= rect.Left;
      while (posX > 30)
      {
        multiplier /= 10;
        posX -= 5;
      }
      value += (delta * multiplier);
      Interface.Handle.SendFloatData(obj.handle, value);
    }

    protected override void SendGuiValue(string value)
    {
      try
      {
        float i = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        Interface.Handle.SendFloatData(obj.handle, i);
      }
      catch (FormatException)
      {

      }

    }
  }
}

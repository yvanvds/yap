using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  static public class Paint
  {
    public static SKPaint ObjectFill = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(60,60,80),
    };

    public static SKPaint ObjectFillSelected = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(80, 80, 100),
    };

    public static SKPaint ObjectBorderHovered = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = new SKColor(180, 180, 220),
    };

    public static SKPaint ObjectBorder = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 4f,
      Color = new SKColor(100, 100, 120),
    };

    public static SKPaint ObjectBorderSelected = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = new SKColor(220, 220, 220),
    };

    public static SKPaint Pin = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      StrokeWidth = 1f,
      Color = new SKColor(155, 155, 175),
    };

    

    public static SKPaint Text = new SKPaint()
    {
      Color = SKColors.White,
      Typeface = SKTypeface.FromFamilyName("arial"),
      TextSize = 12
    };

    public static SKPaint TextInt = new SKPaint()
    {
      Color = SKColors.CadetBlue,
      Typeface = SKTypeface.FromFamilyName("arial"),
      TextSize = 12
    };

    public static SKPaint TextFloatCtrl = new SKPaint()
    {
      Color = SKColors.BlueViolet,
      Typeface = SKTypeface.FromFamilyName("consolas"),
      TextSize = 12
    };

    public static SKPaint Carret = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 2f,
      Color = SKColors.RoyalBlue
    };

    public static SKPaint Connection = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 2f,
      Color = new SKColor(155, 155, 175),
    };

    public static SKPaint ConnectionHover = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 3f,
      Color = new SKColor(155, 155, 220),
    };

    public static SKPaint IntBackground = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = SKColors.CadetBlue
    };

    public static SKPaint FloatBackground = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = SKColors.BlueViolet
    };

    public static SKPaint InvalidBackground = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(204, 134, 14)
    };

    public static SKPaint SliderBackground = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(60, 60, 80),
    };

    public static SKPaint SliderActive = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(94, 150, 237),
    };

    public static SKPaint SliderZero = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(55, 113, 206),
    };

    public static SKPaint SliderHandle = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 1f,
      Color = new SKColor(0, 0, 50),
    };

    public static SKPaint HandlerBackground = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      Color = new SKColor(155, 155, 155),
    };

    public static SKPaint HandlerActiveBackground = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      Color = new SKColor(255, 255, 255),
    };

    public static SKPaint HandlerBorder = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 1f,
      Color = new SKColor(180, 180, 220),
    };

    public static SKPaint ButtonBackground = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = new SKColor(60, 60, 80),
    };

    public static SKPaint ButtonCircle = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 2f,
      Color = new SKColor(80, 80, 100),
    };

    public static SKPaint ButtonBlink = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      Color = new SKColor(94, 150, 237),
    };

    public static SKPaint ToggleOn = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeCap = SKStrokeCap.Round,
      StrokeWidth = 2f,
      Color = new SKColor(94, 150, 237)
    };

    public static SKPaint ToggleOff = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 2f,
      StrokeCap = SKStrokeCap.Round,
      Color = new SKColor(80, 80, 100)
    };

    public static SKPaint MessageBorder = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 2f,
      Color = new SKColor(74, 130, 217),
    };

    public static SKPaint MessageBorderBlink = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 2f,
      Color = new SKColor(94, 150, 237),
    };

    public static SKPaint Message = new SKPaint()
    {
      Color = new SKColor(94, 150, 237),
      Typeface = SKTypeface.FromFamilyName("arial"),
      TextSize = 12
    };
  }
}

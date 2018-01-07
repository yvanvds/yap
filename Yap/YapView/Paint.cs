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
      Color = SKColors.AntiqueWhite,
    };

    public static SKPaint ObjectFillSelected = new SKPaint()
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = SKColors.White,
    };

    public static SKPaint ObjectBorderHovered = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Red,
    };

    public static SKPaint ObjectBorder = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Black,
    };

    public static SKPaint ObjectBorderSelected = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Green,
    };

    public static SKPaint Pin = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      Color = SKColors.Black
    };

    public static SKPaint Text = new SKPaint()
    {
      Color = SKColors.Black,
      Typeface = SKTypeface.FromFamilyName("arial"),
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
      Color = SKColors.Tomato
    };

    public static SKPaint ConnectionHover = new SKPaint()
    {
      Style = SKPaintStyle.Stroke,
      StrokeWidth = 4f,
      Color = SKColors.Red
    };
  }
}

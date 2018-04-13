using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  enum SizeMode
  {
    LOCKED,
    AUTO,
    SQUARE,
    FLEX,
  }

  internal interface ISizer
  {
    SKRect Rect { get; }
    SizeMode Mode { get; }

    float MinWidth { get; set; }
    float MinHeight { get; set; }

    void SetLeft(float value);
    void SetTop(float value);

    bool Show { get; set; }

    bool Contains(SKPoint pos);
    bool OnHandle(SKPoint pos);

    void Move(SKPoint offset);
    void Resize(string text, SKPaint paint);

    void Draw(SKCanvas canvas);

    void OnMouseUp();
  }
}

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  internal interface IConnect
  {
    uint Inputs { get; set; }
    uint Outputs { get; set; }

    int OnInput(SKPoint pos);
    int OnOutput(SKPoint pos);

    SKPoint GetInputPos(uint inlet);
    SKPoint GetOutputPos(uint outlet);

    void Update(SKRect rect);
    void Draw(SKRect rect, SKCanvas canvas);
  }
}

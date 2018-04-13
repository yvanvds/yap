using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace YapView.Gui
{
  internal class ConnectDisabled : IConnect
  {
    uint i, o;
    SKPoint point = new SKPoint(0, 0);

    public uint Inputs { get => 0; set => i = value; }
    public uint Outputs { get => 0; set => o = value; }

    public void Draw(SKRect rect, SKCanvas canvas)
    {

    }

    public SKPoint GetInputPos(uint inlet)
    {
      return point;
    }

    public SKPoint GetOutputPos(uint outlet)
    {
      return point;
    }

    public int OnInput(SKPoint pos)
    {
      return -1;
    }

    public int OnOutput(SKPoint pos)
    {
      return -1;
    }

    public void Update(SKRect rect)
    {
      
    }
  }
}

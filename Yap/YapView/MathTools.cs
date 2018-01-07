using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  public static class MathTools
  {
    public static float FindDistance(SKPoint lineStart, SKPoint lineEnd, SKPoint point, out SKPoint closest)
    {
      float dx = lineEnd.X - lineStart.X;
      float dy = lineEnd.Y - lineStart.Y;
      if(dx == 0 && dy == 0)
      {
        closest = lineStart;
        dx = point.X - lineStart.X;
        dy = point.Y - lineStart.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
      }

      float t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy)
         / (dx * dx + dy * dy);
      if(t < 0)
      {
        closest = lineStart;
        dx = point.X - lineStart.X;
        dy = point.Y - lineStart.Y;
      } else if (t > 1)
      {
        closest = lineEnd;
        dx = point.X - lineEnd.X;
        dy = point.Y - lineEnd.Y;
      } else
      {
        closest = new SKPoint(lineStart.X + t * dx, lineStart.Y + t * dy);
        dx = point.X - closest.X;
        dy = point.Y - closest.Y;
      }

      return (float)Math.Sqrt(dx * dx + dy * dy);
    }


  }
}

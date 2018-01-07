using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YapView
{
  public class Connection
  {
    Object Start = null;
    int StartPin;

    Object End = null;
    int EndPin;

    SKPoint mousePos = new SKPoint();
    SKPoint closest = new SKPoint();

    bool isComplete = false;
    public bool Complete { get => isComplete; }

    public bool Hover { get; set; } = false;

    public void SetStart(Object obj, int pin)
    {
      Start = obj;
      StartPin = pin;
      isComplete = false;
    }

    public void SetEnd(Object obj, int pin)
    {
      End = obj;
      EndPin = pin;
      isComplete = true;
    }

    public void SetMousePos(Point pos)
    {
      mousePos.X = (float)pos.X;
      mousePos.Y = (float)pos.Y;
    }

    public float DistanceToPos(SKPoint pos)
    {
      if (!isComplete) return -1f;
      return MathTools.FindDistance(Start.GetOutputPos(StartPin), End.GetInputPos(EndPin), pos, out closest);
    }

    public void Draw(SKCanvas canvas)
    {
      SKPoint start = Start.GetOutputPos(StartPin);
      SKPoint end;
      if(isComplete)
      {
        end = End.GetInputPos(EndPin);
      } else
      {
        end = mousePos;
      }

      if(Hover)
      {
        canvas.DrawLine(start.X, start.Y, end.X, end.Y, Paint.ConnectionHover);
      }
      else
      {
        canvas.DrawLine(start.X, start.Y, end.X, end.Y, Paint.Connection);
      }
    }

    ~Connection()
    {

    }
  }
}

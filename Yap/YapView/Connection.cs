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
    uint outlet;

    Object End = null;
    uint inlet;

    IYapHandler handler;

    SKPoint mousePos = new SKPoint();
    SKPoint closest = new SKPoint();

    bool isComplete = false;
    public bool Complete { get => isComplete; }

    public bool Hover { get; set; } = false;
    public bool Selected { get; set; } = false;

    public Connection(IYapHandler handler)
    {
      this.handler = handler;
    }

    public void SetStart(Object obj, uint outlet)
    {
      Start = obj;
      this.outlet = outlet;
      isComplete = false;
    }

    public bool IsStart(Object obj)
    {
      return Start == obj;
    }

    public void SetEnd(Object obj, uint inlet, bool connect = true)
    {
      End = obj;
      this.inlet = inlet;
      isComplete = true;

      if(connect) handler.Connect(Start.handle, this.outlet, End.handle, this.inlet);
    }

    public bool IsConnected(Object obj)
    {
      return (End == obj || Start == obj);
    }

    public void SetMousePos(Point pos)
    {
      mousePos.X = (float)pos.X;
      mousePos.Y = (float)pos.Y;
    }

    public float DistanceToPos(SKPoint pos)
    {
      if (!isComplete) return -1f;
      return MathTools.FindDistance(Start.GetOutputPos(outlet), End.GetInputPos(inlet), pos, out closest);
    }

    public void Draw(SKCanvas canvas)
    {
      SKPoint start = Start.GetOutputPos(outlet);
      SKPoint end;
      if (isComplete)
      {
        end = End.GetInputPos(inlet);
      }
      else
      {
        end = mousePos;
      }

      if (Hover || Selected)
      {
        canvas.DrawLine(start.X, start.Y, end.X, end.Y, Paint.ConnectionHover);
      }
      else
      {
        canvas.DrawLine(start.X, start.Y, end.X, end.Y, Paint.Connection);
      }
    }

    public void Disconnect()
    {
      if (isComplete)
      {
        handler.Disconnnect(Start.handle, outlet, End.handle, inlet);
      }
      isComplete = false;
    }

    ~Connection()
    {
      Disconnect();
    }
  }
}

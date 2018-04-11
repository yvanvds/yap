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

    YapView View;

    SKPoint mousePos = new SKPoint();
    SKPoint closest = new SKPoint();

    SKPath path = new SKPath();

    bool isComplete = false;
    public bool Complete { get => isComplete; }

    public bool Hover { get; set; } = false;
    public bool Selected { get; set; } = false;

    public Connection(YapView View)
    {
      this.View = View;
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

      if(connect) Interface.Handle.Connect(Start.handle, this.outlet, End.handle, this.inlet);
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

    public bool Contains(SKPoint pos)
    {
      if (path.Contains(pos.X, pos.Y)) return true;
      return false;
    }

    public float DistanceToPos(SKPoint pos)
    {
      if (!isComplete) return -1f;
      return MathTools.FindDistance(Start.GuiShape.GetOutputPos(outlet), End.GuiShape.GetInputPos(inlet), pos, out closest);
    }

    public void Draw(SKCanvas canvas)
    {
      SKPoint start = Start.GuiShape.GetOutputPos(outlet);
      SKPoint end;
      if (isComplete)
      {
        end = End.GuiShape.GetInputPos(inlet);
      }
      else
      {
        end = mousePos;
      }

      path.Rewind();
      
      path.MoveTo(start.X, start.Y);
      path.CubicTo(start.X, start.Y + 50, end.X, end.Y - 50, end.X, end.Y);

      if (Hover || Selected)
      {
        canvas.DrawPath(path, Paint.ConnectionHover);
      }
      else
      {
        canvas.DrawPath(path, Paint.Connection);
      }
      

      
    }

    public void Disconnect()
    {
      if (isComplete)
      {
        Interface.Handle.Disconnnect(Start.handle, outlet, End.handle, inlet);
      }
      isComplete = false;
    }

    ~Connection()
    {
      Disconnect();
    }
  }
}

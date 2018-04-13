using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YapView
{
  internal class Connection
  {
    WidgetHolder Start = null;
    uint outlet;

    WidgetHolder End = null;
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

    public void SetStart(WidgetHolder obj, uint outlet)
    {
      Start = obj;
      this.outlet = outlet;
      isComplete = false;
    }

    public bool IsStart(WidgetHolder obj)
    {
      return Start == obj;
    }

    public void SetEnd(WidgetHolder obj, uint inlet, bool connect = true)
    {
      End = obj;
      this.inlet = inlet;
      isComplete = true;

      if(connect) Interface.Handle.Connect(Start.Handle, this.outlet, End.Handle, this.inlet);
    }

    public bool IsConnected(WidgetHolder obj)
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
      return MathTools.FindDistance(Start.Widget.Connector.GetOutputPos(outlet), End.Widget.Connector.GetInputPos(inlet), pos, out closest);
    }

    public void Draw(SKCanvas canvas)
    {
      SKPoint start = Start.Widget.Connector.GetOutputPos(outlet);
      SKPoint end;
      if (isComplete)
      {
        end = End.Widget.Connector.GetInputPos(inlet);
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
        Interface.Handle.Disconnnect(Start.Handle, outlet, End.Handle, inlet);
      }
      isComplete = false;
    }

    ~Connection()
    {
      Disconnect();
    }
  }
}

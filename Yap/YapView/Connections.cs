using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YapView
{
  public class Connections
  {
    public Connections(IYapHandler handler)
    {
      this.handler = handler;
    }

    public void Draw(SkiaSharp.SKCanvas canvas)
    {
      foreach (var obj in List)
      {
        obj.Draw(canvas);
      }
    }

    public void Deselect()
    {
      if (Current != null) Current.Selected = false;
    }

    public bool TrySelectBelowMouse()
    {
      if(BelowMouse != null)
      {
        Current = BelowMouse;
        Current.Selected = true;
        return true;
      }
      return false;
    }

    public void Add(Object obj, int pin, SKPoint pos)
    {
      List.Add(new Connection(handler));
      Current = List.Last();
      Current.SetStart(obj, pin);
      Point p = new Point();
      p.X = pos.X;
      p.Y = pos.Y;
      Current.SetMousePos(p);
    }

    public bool IsBeingCreated()
    {
      return (Current != null && Current.Complete == false);
    }

    public void DeleteCurrent()
    {
      if(Current != null)
      {
        Current.Disconnect();
        List.Remove(Current);
        Current = null;
      }
    }

    public void TrySetCurrentEnd(Object obj, SKPoint pos)
    {
      if (Current.IsStart(obj)) return;
      int pin = obj.OnInput(pos);
      if (pin == -1 && obj.Inputs > 0)
      {
        pin = 0;
      }
      if (pin == -1)
      {
        DeleteCurrent();
      }
      else
      {
        Current.SetEnd(obj, pin);
        Current = null;
      }
    }

    public void RemoveConnectedTo(Object obj)
    {
      for (int i = List.Count - 1; i >= 0; i--)
      {
        if (List[i].IsConnected(obj))
        {
          List.RemoveAt(i);
        }
      }
    }

    public bool SetBelowMouse(SKPoint pos)
    {
      BelowMouse = null;
      if (List.Count > 0)
      {
        Connection nearest = List[0];
        float nearestDist = float.MaxValue;
        foreach (var obj in List)
        {
          obj.Hover = false;
          if (obj.Complete)
          {
            float dist = obj.DistanceToPos(pos);
            if (dist < nearestDist)
            {
              nearestDist = dist;
              nearest = obj;
            }
          }
        }

        if (nearestDist < 5)
        {
          BelowMouse = nearest;
          BelowMouse.Hover = true;
          return true;
        }
      }
      return false;
    }

    ObservableCollection<Connection> List = new ObservableCollection<Connection>();
    IYapHandler handler;
    public Connection Current { set; get; } = null;
    public Connection BelowMouse { set; get; } = null;
  }
}

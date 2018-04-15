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
  internal class Connections
  {
    ObservableCollection<Connection> List = new ObservableCollection<Connection>();
    YapView View;

    public Connection Current { set; get; } = null;
    public Connection BelowMouse { set; get; } = null;

    public Connections(YapView View)
    {
      this.View = View;
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

    public void Add(WidgetHolder obj, uint outlet, SKPoint pos)
    {
      List.Add(new Connection(View));
      Current = List.Last();
      Current.SetStart(obj, outlet);
      Point p = new Point
      {
        X = pos.X,
        Y = pos.Y
      };
      Current.SetMousePos(p);
    }

    public void Add(WidgetHolder start, uint outlet, WidgetHolder end, uint inlet)
    {
      List.Add(new Connection(View));
      List.Last().SetStart(start, outlet);
      List.Last().SetEnd(end, inlet);
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

    public void Delete(Connection conn)
    {
      conn.Disconnect();
      List.Remove(conn);
    }

    public void Clear()
    {
      foreach(var connection in List)
      {
        connection.Disconnect();
      }
      BelowMouse = null;
      Current = null;
      List.Clear();
    }

    public void TrySetCurrentEnd(WidgetHolder obj, SKPoint pos)
    {
      if (Current.IsStart(obj)) return;
      int pin = obj.Widget.Connector.OnInput(pos);
      if (pin == -1 && obj.Widget.Connector.Inputs > 0)
      {
        pin = 0;
      }
      if (pin == -1)
      {
        DeleteCurrent();
      }
      else
      {
        Current.SetEnd(obj, (uint)pin);
        Current = null;
      }
    }

    public void RemoveConnectedTo(WidgetHolder obj)
    {
      for (int i = List.Count - 1; i >= 0; i--)
      {
        if (List[i].IsConnected(obj))
        {
          List[i].Disconnect();
          List.RemoveAt(i);
        }
      }
    }

    public bool SetBelowMouse(SKPoint pos)
    {
      BelowMouse = null;
      foreach(var obj in List)
      {
        obj.Hover = false;
        if(obj.Contains(pos))
        {
          BelowMouse = obj;
          BelowMouse.Hover = true;
          return true;
        }
      }

      // nothing found.Try and find nearby lines
      // This is needed because the detect method above
      // does not seem to work for straight lines

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


  }
}

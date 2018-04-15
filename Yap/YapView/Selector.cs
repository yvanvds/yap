using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView
{
  internal class Selector
  {
    public List<WidgetHolder> Widgets = new List<WidgetHolder>();
    public List<Connection> Connections = new List<Connection>();

    WidgetHolder newWidget = null;
    Connection newConnection = null;

    public bool MultiSelect { get; set; } = false;

    YapView view;

    public Selector(YapView view)
    {
      this.view = view;
    }

    public void Select(WidgetHolder holder)
    {
      if (!MultiSelect)
      {
        newWidget = holder;
        Clear();
        newWidget = null;
      }
      holder.Widget.Selected = true;
      Widgets.Add(holder);
    }

    public void Select(Connection conn)
    {
      if (!MultiSelect)
      {
        newConnection = conn;
        Clear();
        newConnection = null;
      }
      conn.Selected = true;
      Connections.Add(conn);
    }

    public void Clear()
    {
      foreach(var conn in Connections)
      {
        if(conn != newConnection) conn.Selected = false;
      }
      Connections.Clear();

      foreach(var holder in Widgets)
      {
        if(holder != newWidget) holder.Widget.Deselect();
      }
      Widgets.Clear();
    }

    private void DeleteAll()
    {
      foreach (var conn in Connections)
      {
        view.Connections.Delete(conn);
      }
      Connections.Clear();

      foreach (var holder in Widgets)
      {
        view.Connections.RemoveConnectedTo(holder);
        view.Widgets.Delete(holder);
      }
      Widgets.Clear();
    }

    public void CanvasClicked()
    {
      if(!MultiSelect)
      {
        Clear();
      }
    }

    public void OnMouseMove(MouseEventArgs e)
    {
      foreach(var holder in Widgets)
      {
        holder.Widget.OnMouseMove(e);
      }
    }

    public void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      foreach(var holder in Widgets)
      {
        holder.Widget.OnMouseUp(e);
      }
    }

    public bool OnKeyDown(KeyEventArgs e)
    {
      foreach(var holder in Widgets)
      {
        if(holder.Widget.Keyboard.Focus)
        {
          if(holder.Widget.Keyboard.HandleKeyDown(e))
          {
            return true;
          }
        }
      }

      if(e.Key == Key.Delete || e.Key == Key.Back)
      {
        DeleteAll();
        return true;
      }

      if(e.Key == Key.LeftCtrl)
      {
        MultiSelect = true;
        return true;
      }

      return false;
    }

    public bool OnKeyUp(KeyEventArgs e)
    {
      if(e.Key == Key.LeftCtrl)
      {
        MultiSelect = false;
        return true;
      }
      return false;
    }
  }
}

using SkiaSharp;
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

    SkiaSharp.SKRect SelectRect = new SkiaSharp.SKRect();
    bool MouseDownSelectActive = false;
    bool CtrlDown = false;
    SkiaSharp.SKPoint PreviousMousePos = new SkiaSharp.SKPoint();

    YapView view;

    public Selector(YapView view)
    {
      this.view = view;
    }

    public void Select(WidgetHolder holder)
    {
      foreach(var widget in Widgets)
      {
        if (widget.Widget.Selected && widget == holder) return;
      }

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

    public void CanvasClicked(MouseButtonEventArgs e)
    {
      if(!MultiSelect)
      {
        Clear();
      }

      MouseDownSelectActive = true;
      SelectRect.Left = (float)e.GetPosition(view).X;
      SelectRect.Top = (float)e.GetPosition(view).Y;
      SelectRect.Size = new SkiaSharp.SKSize(0, 0);

      PreviousMousePos.X = (float)e.GetPosition(view).X;
      PreviousMousePos.Y = (float)e.GetPosition(view).Y;
    }

    public void OnMouseMove(MouseEventArgs e)
    {
      if(!MouseDownSelectActive) {
        if(Widgets.Count < 2)
        {
          foreach(var holder in Widgets)
          {
            holder.Widget.OnMouseMove(e);
          }
          return;
        }
        SKPoint pos = new SKPoint
        {
          X = (float)e.GetPosition(view).X,
          Y = (float)e.GetPosition(view).Y
        };
        if(PreviousMousePos.X != 0 || PreviousMousePos.Y != 0)
        {
          foreach (var holder in Widgets)
          {
            holder.Widget.Move(pos - PreviousMousePos);
          }
        }
        PreviousMousePos = pos;
      }

      if(MouseDownSelectActive)
      {
        SkiaSharp.SKSize size = new SkiaSharp.SKSize(
          width: (float)e.GetPosition(view).X - SelectRect.Left,
          height: (float)e.GetPosition(view).Y - SelectRect.Top
        );

        SelectRect.Size = size;

        if(size.Width != 0 || size.Height != 0)
        {
          MultiSelect = true;
        } else
        {
          if(!CtrlDown) MultiSelect = false;
        }

        Widgets.Clear();
        foreach(var holder in view.Widgets.List)
        {
          if(holder.Widget.IsInside(SelectRect))
          {
            Select(holder);
          }
        }
      }
    }

    public void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      MouseDownSelectActive = false;
      if(!CtrlDown) MultiSelect = false;
      foreach(var holder in Widgets)
      {
        holder.Widget.OnMouseUp(e);
      }
      PreviousMousePos.X = PreviousMousePos.Y = 0;
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
        CtrlDown = true;
        MultiSelect = true;
        return true;
      }

      return false;
    }

    public bool OnKeyUp(KeyEventArgs e)
    {
      if(e.Key == Key.LeftCtrl)
      {
        CtrlDown = false;
        MultiSelect = false;
        return true;
      }
      return false;
    }

    public void Draw(SkiaSharp.SKCanvas canvas)
    {
      if(MouseDownSelectActive)
      {
        canvas.DrawRect(SelectRect, Paint.Selector);
      }
    }
  }
}

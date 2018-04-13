using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  internal class Widgets
  {
    internal ObservableCollection<WidgetHolder> List = new ObservableCollection<WidgetHolder>();
    YapView View;

    internal Widgets(YapView View)
    {
      this.View = View;
    }

    internal WidgetHolder Add(SkiaSharp.SKPoint pos, object handle = null)
    {
      List.Add(new WidgetHolder(View, handle, pos));
      return List.Last();
    }

    internal void Delete(WidgetHolder holder)
    {
      holder.Widget.Release();
      List.Remove(holder);
    }

    internal void Update()
    {
      foreach(var holder in List)
      {
        holder.Update();
      }
    }

    internal void Draw(SkiaSharp.SKCanvas canvas)
    {
      foreach(var holder in List)
      {
        holder.Widget.Draw(canvas);
      }
    }

    internal void Deselect()
    {
      foreach(var holder in List)
      {
        holder.Widget.Selected = false;
      }
    }

    internal void Clear()
    {
      foreach(var holder in List)
      {
        holder.Widget.Release();
      }
      List.Clear();
    }

    internal WidgetHolder At(SkiaSharp.SKPoint pos)
    {
      foreach(var holder in List)
      {
        if (holder.Widget.Contains(pos)) return holder;
      }
      return null;
    }

    internal WidgetHolder Get(uint ID)
    {
      foreach(var holder in List)
      {
        if (holder.ID == ID) return holder;
      }
      return null;
    }

    internal void Save()
    {
      foreach(var holder in List)
      {
        holder.Widget.Save();
      }
    }
  }
}

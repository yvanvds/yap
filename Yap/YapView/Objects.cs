using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  public class Objects
  {
    public Objects(IYapHandler Handler)
    {
      this.Handler = Handler;
    }

    public void Draw(SkiaSharp.SKCanvas canvas)
    {
      foreach (var obj in List)
      {
        obj.Draw(canvas);
      }
    }

    public Object Add(SkiaSharp.SKPoint pos)
    {
      List.Add(new Object(pos, Handler));
      Current = List.Last();
      return Current;
    }

    public bool TrySetCurrent()
    {
      if(BelowMouse != null)
      {
        Current = BelowMouse;
        return true;
      }
      return false;
    }

    public void Deselect()
    {
      if (Current != null) Current.Selected = false;
      Current = null;
    }

    public void SelectCurrent()
    {
      if (Current == null) return;
      Current.Selected = true;
      int index = List.IndexOf(Current);
      List.Move(index, List.Count - 1);
    }

    public bool CurrentIsSelected()
    {
      if (Current == null) return false;
      return Current.Selected;
    }

    public int EditCurrentOnPos(SkiaSharp.SKPoint pos)
    {
      if (Current == null) return 0;
      Current.EditMode = true;
      Current.CarretPos = Current.FindCarretPos(pos);
      return Current.CarretPos;
    }

    public void EndEditMode()
    {
      if (Current == null) return;
      Current.Selected = false;
      Current.EditMode = false;
    }

    public bool InEditMode()
    {
      if (Current == null) return false;
      return Current.EditMode;
    }

    public void UpdateEditMode(string text, int carretPos)
    {
      if (Current == null) return;
      Current.Text = text;
      Current.CarretPos = carretPos;
    }

    public void SetBelowMouse(SkiaSharp.SKPoint pos)
    {
      BelowMouse = null;
      if (List.Count > 0)
      {
        bool HoverFound = false;
        for (int i = List.Count - 1; i >= 0; i--)
        {
          if (!HoverFound)
          {
            if (List[i].IsInside(pos))
            {
              BelowMouse = List[i];
              HoverFound = true;
            }
          }
          List[i].Hover = false;
        }
      }
    }

    public void DeleteCurrent()
    {
      if (Current == null) return;
      List.Remove(Current);
      Current = null;
    }

    public Object Current { get; set; } = null;
    public Object BelowMouse { get; set; } = null;
    public ObservableCollection<Object> List = new ObservableCollection<Object>();
    IYapHandler Handler = null;
  }
}

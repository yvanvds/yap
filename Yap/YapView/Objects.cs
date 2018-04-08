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
    public Object Current { get; set; } = null;
    public Object BelowMouse { get; set; } = null;
    public ObservableCollection<Object> List = new ObservableCollection<Object>();
    YapView View = null;

    public Objects(YapView View)
    {
      this.View = View;
    }

    public Object GetObjectWithID(uint objID)
    {
      foreach(Object obj in List)
      {
        if (obj.ObjID == objID) return obj;
      }

      return null;
    }

    public void UpdateGui()
    {
      foreach (var obj in List)
      {
        obj.GuiShape.UpdateGui();
      }
    }

    public void Draw(SkiaSharp.SKCanvas canvas)
    {
      foreach (var obj in List)
      {
        obj.Draw(canvas);
      }
    }

    /*
        SELECTION
    */

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
      if (Current != null)
      {
        Current.GuiShape.Selected = false;
        Current.GuiShape.EditMode = false;
        Current.GuiShape.GuiEditMode = false;
      }
      Current = null;
    }

    public void SelectCurrent()
    {
      if (Current == null) return;
      Current.GuiShape.Selected = true;
      int index = List.IndexOf(Current);
      List.Move(index, List.Count - 1);
    }

    public bool CurrentIsSelected()
    {
      if (Current == null) return false;
      return Current.GuiShape.Selected;
    }

    /*
        EDIT
    */

    public int EditCurrentOnPos(SkiaSharp.SKPoint pos)
    {
      if (Current == null) return 0;
      Current.GuiShape.EditMode = true;
      return Current.GuiShape.SetCarretPos(pos);
    }

    public void EndEditMode()
    {
      if (Current == null) return;
      Current.GuiShape.Selected = false;
      Current.GuiShape.EditMode = false;
    }

    public bool InEditMode()
    {
      if (Current == null) return false;
      return Current.GuiShape.EditMode;
    }

    public void UpdateEditMode(string text, int carretPos)
    {
      if (Current == null) return;
      Current.GuiShape.Text = text;
      Current.GuiShape.CarretPos = carretPos;
    }

    public void SetBelowMouse(SkiaSharp.SKPoint pos)
    {
      BelowMouse = null;
      if (List.Count > 0)
      {
        bool HoverFound = false;
        for (int i = List.Count - 1; i >= 0; i--)
        {
          List[i].GuiShape.Hover = false;
          if (!HoverFound)
          {
            if (List[i].GuiShape.IsInside(pos))
            {
              BelowMouse = List[i];
              HoverFound = true;
              List[i].GuiShape.Hover = true;
            }
          }
          
        }
      }
    }

    /*
        CONTAINER MANIPULATION
    */

    public Object Add(SkiaSharp.SKPoint pos, object obj = null)
    {
      List.Add(new Object(pos, View, obj));
      Current = List.Last();
      return Current;
    }

    public void DeleteCurrent()
    {
      if (Current == null) return;
      Current.Release();
      List.Remove(Current);
      Current = null;
    }

    public void StorePositions()
    {
      foreach(var obj in List)
      {
        obj.StorePosition();
      }
    }

    public void Clear()
    {
      Current = null;
      BelowMouse = null;
      foreach(var item in List)
      {
        item.Release();
      }
      List.Clear();
    }

    
  }
}

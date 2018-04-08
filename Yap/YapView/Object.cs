using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace YapView
{

  public class Object
  {
    YapView view;
    public YapView View { get => view; }

    StringEditor editor = new StringEditor();
    public StringEditor Editor { get => editor; }

    public object handle = null;
    public Gui.Base GuiShape;


    string currentObjectName = "";
    uint objID;
    ObjectType Type = ObjectType.BASE;
    public uint ObjID { get => objID; }

    public Object(SKPoint pos, YapView View, object handle)
    {
      this.view = View;
      this.handle = handle;

      GuiShape = new Gui.BasicCtrl(pos, this);
    }

    public void GetValuesFromHandle()
    {
      string name = View.Handler.GetObjectName(handle);
      string args = View.Handler.GetObjectArguments(handle);

      ObjectType newType = View.Handler.GetObjectType(handle);
      if (newType != Type)
      {
        Type = newType;
        SwitchGui();
      }

      if (args.Length > 0) name += " " + args;
      GuiShape.Text = name;
      currentObjectName = name;
      GuiShape.Inputs = View.Handler.GetObjectInputCount(handle);
      GuiShape.Outputs = View.Handler.GetObjectOutputCount(handle);
      objID = View.Handler.GetObjectID(handle);
      GuiShape.Selected = false;
      GuiShape.EditMode = false;
      UpdateLayout();
    }

    public void Move(SKPoint delta)
    {
      GuiShape.Move(delta);
    }

    public void OnMouseLeftButtonDown(SKPoint pos)
    {
      GuiShape.OnMouseDown(pos);
    }

    public void OnMouseMove(SKPoint pos)
    {
      GuiShape.OnMouseMove(pos);
    }

    public void OnMouseLeftButtonUp(SKPoint pos)
    {
      GuiShape.OnMouseUp(pos);
    }

    public void OnMouseWheel(MouseWheelEventArgs e, float pos)
    {
      GuiShape.OnMouseWheel(e, pos);
    }

    public void StorePosition()
    {
      View.Handler.SetPosition(handle, GuiShape.Rect.Left, GuiShape.Rect.Top);
    }

    public bool HasChanged()
    {
      return !GuiShape.Text.Equals(currentObjectName);
    }

    public bool Reconfigure() {
      // separate object name from arguments
      string[] original = currentObjectName.Split(' ');
      string[] newText = GuiShape.Text.Split(' ');
      currentObjectName = GuiShape.Text;
      bool objectIsReplaced = false;

      if (handle != null)
      {
        if(newText.Length > 0)
        {
          // reconstruct object if name is different
          if(!original[0].Equals(newText[0]))
          {
            View.Handler.DeleteObject(handle);
            handle = View.Handler.CreateObject(GuiShape.Text);
            objectIsReplaced = true;
          }
        } else
        {
          View.Handler.DeleteObject(handle);
          handle = null;
          objectIsReplaced = true;
        }
      } else
      {
        // handle is null, create a new object if possible
        if(newText.Length > 0)
        {
          handle = View.Handler.CreateObject(newText[0]);
          objectIsReplaced = true;
        }
      }

      if (handle != null)
      {
        // pass arguments
        var args = currentObjectName.Split(new[] { ' ' }, 2);
        if(args.Length > 1)
        {
          View.Handler.PassArgument(handle, args[1]);
        } else
        {
          View.Handler.PassArgument(handle, "");
        }

        ObjectType newType = View.Handler.GetObjectType(handle);
        if (newType != Type)
        {
          Type = newType;
          SwitchGui();
        }

        // set inlets and outlets
        GuiShape.Inputs = View.Handler.GetObjectInputCount(handle);
        GuiShape.Outputs = View.Handler.GetObjectOutputCount(handle);
        objID = View.Handler.GetObjectID(handle);
      } else
      {
        Type = ObjectType.BASE;
        SwitchGui();
        GuiShape.Inputs = 0;
        GuiShape.Outputs = 0;
      }
      
      UpdateLayout();

      return objectIsReplaced;
    }

    void SwitchGui()
    {
      string Text = GuiShape.Text;
      switch(Type)
      {
        case ObjectType.FLOAT:
          {
            GuiShape = new Gui.FloatCtrl(GuiShape.Rect.Location, this);
            break;
          }
        case ObjectType.INT:
          {
            GuiShape = new Gui.IntCtrl(GuiShape.Rect.Location, this);
            break;
          }
        case ObjectType.SLIDER:
          {
            GuiShape = new Gui.SliderCtrl(GuiShape.Rect.Location, this);
            break;
          }
        case ObjectType.BUTTON:
          {
            GuiShape = new Gui.ButtonCtrl(GuiShape.Rect.Location, this);
            break;
          }
        case ObjectType.TOGGLE:
          {
            GuiShape = new Gui.ToggleCtrl(GuiShape.Rect.Location, this);
            break;
          }
        default:
          {
            GuiShape = new Gui.BasicCtrl(GuiShape.Rect.Location, this);
            break;
          }   
      }
      GuiShape.Text = Text;
      GuiShape.Selected = false;
      GuiShape.EditMode = false;
    }
 
    public void Draw(SKCanvas canvas)
    {
      GuiShape.Draw(canvas);
    }

    private void UpdateLayout()
    {
      GuiShape.Update();
    }

    public string GetGuiValue()
    {
      if (handle == null) return "";
      return View.Handler.GetGuiValue(handle);
    }

    public void Deselect()
    {
      GuiShape.Selected = false;
      GuiShape.GuiEditMode = false;
      GuiShape.EditMode = false;
    }

    public void Release()
    {
      if(handle != null)
      {
        View.Handler.DeleteObject(handle);
      }
      handle = null;
    }
  }
}

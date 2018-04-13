using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{

  internal class WidgetHolder
  {
    internal Gui.Widget Widget;
    YapView parent;

    object handle = null;
    public object Handle { get => handle; }

    ObjectType Type = ObjectType.INVALID;

    string name = "";
    string args = "";

    public uint ID { get; set; }
    

    public WidgetHolder(YapView parent, object handle, SKPoint pos)
    {
      this.parent = parent;
      this.handle = handle;
      Widget = new Gui.InvalidObject(parent, handle, pos);
    }

    public void Load()
    {
      ObjectType newType = ObjectType.INVALID;

      if(handle != null)
      {
        name = Interface.Handle.GetObjectName(handle);
        args = Interface.Handle.GetObjectArguments(handle);
        ID = Interface.Handle.GetObjectID(handle);
        newType = Interface.Handle.GetObjectType(handle);
      } 
      
      if (newType != Type)
      {
        Type = newType;
        SetWidget();
      }
      

      if (handle != null)
      {
        Widget.Name = name;
        if (args.Length > 0)
        {
          Widget.Name += " " + args;
        }
        Widget.Load();
      }
    }

    public void SetWidget()
    {
      switch(Type)
      {
        case ObjectType.FLOAT:
          {
            Widget = new Gui.FloatObject(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.INT:
          {
            Widget = new Gui.IntObject(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.SLIDER:
          {
            Widget = new Gui.Slider(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.BUTTON:
          {
            Widget = new Gui.Button(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.TOGGLE:
          {
            Widget = new Gui.Toggle(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.COUNTER:
          {
            Widget = new Gui.Counter(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.MESSAGE:
          {
            Widget = new Gui.Message(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        case ObjectType.INVALID:
          {
            Widget = new Gui.InvalidObject(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
        default:
          {
            Widget = new Gui.Connectable(parent, handle, Widget.Sizer.Rect.Location);
            break;
          }
      }
    }

    public void Update()
    {
      if(Widget.NeedsEvaluation)
      {
        Widget.NeedsEvaluation = false;
        string[] newName = Widget.Name.Split(new[] { ' ' }, 2);
        string widgetName = Widget.Name;

        if(handle != null)
        {
          if(newName.Length > 0)
          {
            // reconstruct widget if name is different
            if(!newName[0].Equals(name))
            {
              Widget.Release();
              handle = Interface.Handle.CreateObject(newName[0]);
            }
          } else
          {
            Widget.Release();
            handle = null;
          }
        } else
        {
          // handle is null, create a new object if possible
          if(newName.Length > 0)
          {
            handle = Interface.Handle.CreateObject(newName[0]);
          }
        }

        if (handle != null)
        {
          if (newName.Length > 1)
          {
            Interface.Handle.PassArgument(handle, newName[1]);
          }
          else
          {
            Interface.Handle.PassArgument(handle, "");
          }
        } 

        // reload object
        Load();

        if (handle == null)
        {
          // set name manually
          Widget.Name = widgetName;
        }
      }

      Widget.Update();
    }
  }
}

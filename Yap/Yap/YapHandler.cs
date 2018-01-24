using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YSE;

namespace Yap
{
  class YapHandler : YapView.IYapHandler
  {
    public YSE.IPatcher patcher = null;

    public YapHandler(YSE.IPatcher patcher)
    {
      this.patcher = patcher;
    }
    
    public void Load(string jsonContent)
    {
      patcher.ParseJSON(jsonContent);

    }

    public string Save()
    {
      return patcher.DumpJSON();
    }

    public void Clear()
    {
      patcher.Clear();
    }

    public void Connect(object start, uint outlet, object end, uint inlet)
    {
      patcher.Connect((IHandle)start, (int)outlet, (IHandle)end, (int)inlet);
    }

    public object CreateObject(string name)
    {
      return patcher.CreateObject(name);
    }

    public void DeleteObject(object obj)
    {
      patcher.DeleteObject((IHandle)obj);
    }

    public void Disconnnect(object start, uint outlet, object end, uint inlet)
    {
      patcher.Disconnect((IHandle)start, (int)outlet, (IHandle)end, (int)inlet);
    }

    public int GetObjectInputCount(object name)
    {
      return ((IHandle)name).Inputs;
    }
    public int GetObjectOutputCount(object name)
    {
        return ((IHandle)name).Outputs;
    }

    public void PassArgument(object name, string args)
    {
      ((IHandle)name).SetArgs(args);
    }

    public void GetPosition(object obj, out float X, out float Y)
    {
      Pos p = ((IHandle)obj).GetPosition();
      X = p.X;
      Y = p.Y;
    }

    public void SetPosition(object obj, float X, float Y)
    {
      Pos p = new Pos();
      p.X = X;
      p.Y = Y;
      ((IHandle)obj).SetPosition(p);
    }

    public uint NumObjects()
    {
      return patcher.NumObjects();
    }

    public object GetObjectFromList(uint obj)
    {
      return patcher.GetHandleFromList(obj);
    }

    public string GetObjectName(object obj)
    {
      return ((IHandle)obj).Name;
    }

    public string GetObjectArguments(object obj)
    {
      return ((IHandle)obj).GetArgs();
    }

    public uint GetConnections(object obj, uint outlet)
    {
      return ((IHandle)obj).GetConnections(outlet);
    }

    public uint GetConnectionTarget(object obj, uint outlet, uint connection)
    {
      return ((IHandle)obj).GetConnectionTarget(outlet, connection);
    }

    public uint GetConnectionTargetInlet(object obj, uint outlet, uint connection)
    {
      return ((IHandle)obj).GetConnectionTargetInlet(outlet, connection);
    }

    public uint GetObjectID(object obj)
    {
      return ((IHandle)obj).GetID();
    }
  }
}

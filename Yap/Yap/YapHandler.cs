using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    

    public void Connect(object start, int outlet, object end, int inlet)
    {
      patcher.Connect((IHandle)start, outlet, (IHandle)end, inlet);
    }

    public object CreateObject(string name)
    {
      return patcher.CreateObject(name);
    }

    public void DeleteObject(object obj)
    {
      patcher.DeleteObject((IHandle)obj);
    }

    public void Disconnnect(object start, int outlet, object end, int inlet)
    {
      patcher.Disconnect((IHandle)start, outlet, (IHandle)end, inlet);
    }

    public int GetObjectInputCount(object name)
    {
      return ((IHandle)name).Inputs;
    }
    public int GetObjectOutputCount(object name)
    {
        return ((IHandle)name).Outputs;
    }

    public void PassArgument(object obj, uint pos, float value)
    {
      ((IHandle)obj).SetParam(pos, value);
    }
  }
}

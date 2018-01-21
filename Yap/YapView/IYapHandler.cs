using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  public interface IYapHandler
  {
    int GetObjectInputCount(object name);
    int GetObjectOutputCount(object name);

    object CreateObject(string name);
    void DeleteObject(object obj);

    void Connect(object start, int pinStart, object end, int pinEnd);
    void Disconnnect(object start, int pinStart, object end, int pinEnd);

    void PassArgument(object obj, uint pos, float value);
  }
}

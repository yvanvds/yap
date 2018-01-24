using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YapView
{
  public interface IYapHandler
  {
    void Load(string jsonContent);
    string Save();
    void Clear();

    object CreateObject(string name);
    void DeleteObject(object obj);

    void Connect(object start, uint outlet, object end, uint inlet);
    void Disconnnect(object start, uint outlet, object end, uint inlet);

    void PassArgument(object obj, string args);

    void GetPosition(object obj, out float X, out float Y);
    void SetPosition(object obj, float X, float Y);

    uint NumObjects();
    object GetObjectFromList(uint obj);

    uint GetObjectID(object obj);
    string GetObjectName(object obj);
    string GetObjectArguments(object obj);
    int GetObjectInputCount(object obj);
    int GetObjectOutputCount(object obj);

    uint GetConnections(object obj, uint outlet);
    uint GetConnectionTarget(object obj, uint outlet, uint connection);
    uint GetConnectionTargetInlet(object obj, uint outlet, uint connection);
  }
}

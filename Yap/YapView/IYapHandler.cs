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

    string GetGuiProperty(object obj, string key);
    void SetGuiProperty(object obj, string key, string value);

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

    void SendBang(object obj);
    void SendIntData(object obj, int value);
    void SendFloatData(object obj, float value);
    void SendStringData(object obj, string value);

    string GetGuiValue(object obj);
    ObjectType GetObjectType(object obj);
  }
}

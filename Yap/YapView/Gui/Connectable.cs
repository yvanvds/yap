using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView.Gui
{
  class Connectable : Widget
  {
    public Connectable(YapView parent, object handle, SKPoint pos)
      : base(parent, handle, pos)
    {
      Connector = new ConnectEnabled();
    }


  }
}

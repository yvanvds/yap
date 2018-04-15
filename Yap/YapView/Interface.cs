using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  public static class Interface
  {
    /* This interface is used by yapview to interact with 
     * an underlying object manager of your choice.
     * It MUST be implemented and set before using any
     * yap classes.
     */
    

    static bool performanceMode = false;
    public static bool PerformanceMode { get => performanceMode; set => performanceMode = value; }
  }
}

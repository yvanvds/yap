using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapView
{
  internal static class Convert
  {
    internal static float ToFloat(string s)
    {
      try
      {
        return float.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
      }
      catch (FormatException)
      {
        return 0f;
      }
    }

    internal static int ToInt(string s)
    {
      try
      {
        return Int32.Parse(s);
      }
      catch (FormatException)
      {
        return 0;
      }
    }
  }
}

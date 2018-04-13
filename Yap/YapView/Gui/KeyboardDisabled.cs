using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  internal class KeyboardDisabled : IKeyboard
  {
    bool focus;
    public bool Focus { get => false; set => focus = false; }

    public string Text => "";

    public void Init(string text, SKPoint mousepos, SKRect rect, SKPaint paint)
    {

    }

    public void Edit(EditModeStyle mode)
    {

    }

    public bool AcceptsInput()
    {
      return false;
    }

    public bool HandleKeyDown(KeyEventArgs e)
    {
      return false;
    }

    public bool HandleKeyUp(KeyEventArgs e)
    {
      return false;
    }

    public void Draw(SkiaSharp.SKRect rect, SkiaSharp.SKCanvas canvas)
    {

    }
  }
}

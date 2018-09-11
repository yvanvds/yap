using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  internal interface IKeyboard
  {
    bool AcceptsInput();
    bool Focus { get; set; }

    string Text { get; }

    void Init(string text, SKPoint mousepos, SKRect rect, SKPaint paint);
    void Edit(EditModeStyle mode);

    bool HandleKeyDown(KeyEventArgs e);
    bool HandleKeyUp(KeyEventArgs e);

    void Draw(SKRect rect, SKCanvas canvas);
  }
}

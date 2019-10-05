using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
    internal class KeyboardEnabled : IKeyboard
    {
        bool focus;
        public bool Focus
        {
            get => focus;
            set => focus = value;
        }

        string text;
        public string Text { get => text; }

        StringEditor Editor = new StringEditor();

        public int CarretPos;

        SKPaint paint;

        public void Init(string text, SKPoint mousepos, SKRect rect, SKPaint paint)
        {
            this.paint = paint;
            this.text = text;
            string s = text;


            float length = s.Length > 0 ? paint.MeasureText(s) : 0f;
            while (s.Length > 0 && mousepos.X < rect.Left + 4 + length)
            {
                s = s.Remove(s.Length - 1);
                length = paint.MeasureText(s);
            }
            CarretPos = s.Length;
        }

        public void Edit(EditModeStyle mode = EditModeStyle.Line)
        {
            Editor.Edit(Text, CarretPos, mode);
        }

        public bool AcceptsInput()
        {
            return true;
        }

        public bool HandleKeyDown(KeyEventArgs e)
        {
            if (Focus)
            {
                this.text = Editor.Update(e.Key);
                CarretPos = Editor.Pos;
                e.Handled = true;
                return true;
            }
            return false;
        }

        public bool HandleKeyUp(KeyEventArgs e)
        {
            return false;
        }

        public void Draw(SkiaSharp.SKRect rect, SkiaSharp.SKCanvas canvas)
        {
            canvas.DrawText(Text, rect.Left + 4, rect.Top + 17, paint);
            string s = Text;
            if (CarretPos < Text.Length) s = s.Remove(CarretPos);

            float xPos = rect.Left + 4;
            if (s.Length > 0)
            {
                xPos += paint.MeasureText(s);
            }

            canvas.DrawLine(xPos, rect.Top + 5, xPos, rect.Bottom - 4, Paint.Carret);
        }
    }
}

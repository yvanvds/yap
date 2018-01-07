﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView
{
  public class StringEditor
  {
    string Content;
    public int Pos { get; set; }
    IoCmd_t data;

    public void Edit(string content, int position)
    {
      Content = content;
      Pos = position;
    }

    public string Update(Key key)
    {
      KeyToChar(key, ref data);
      if(data.printable)
      {
        string s = data.character.ToString();
        Content = Content.Insert(Pos,s);
        Pos++;
      } else
      {
        if(data.arrowLeft)
        {
          Pos--;
          if (Pos < 0) Pos = 0;
        } else if (data.arrowRight)
        {
          Pos++;
          if (Pos > Content.Length) Pos = Content.Length;
        } else if(data.backspace)
        {
          if (Pos > 0)
          {
            Content = Content.Remove(Pos - 1, 1);
            Pos--;
          }
        } else if(data.delete)
        {
          if(Pos < Content.Length)
          {
            Content = Content.Remove(Pos, 1);
          }
        }
      }

      return Content;
    }

    private void KeyToChar(Key key, ref IoCmd_t KeyDecode)
    {
      bool iscap;
      bool caplock;
      bool shift;

      KeyDecode.key = key;

      KeyDecode.alt = Keyboard.IsKeyDown(Key.LeftAlt) ||
                        Keyboard.IsKeyDown(Key.RightAlt);

      KeyDecode.ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) ||
                        Keyboard.IsKeyDown(Key.RightCtrl);

      KeyDecode.shift = Keyboard.IsKeyDown(Key.LeftShift) ||
                        Keyboard.IsKeyDown(Key.RightShift);

      if (KeyDecode.alt || KeyDecode.ctrl)
      {
        KeyDecode.printable = false;
        KeyDecode.type = 1;
      }
      else
      {
        KeyDecode.printable = true;
        KeyDecode.type = 0;
      }

      shift = KeyDecode.shift;
      caplock = Console.CapsLock; //Keyboard.IsKeyToggled(Key.CapsLock);
      iscap = (caplock && !shift) || (!caplock && shift);

      KeyDecode.backspace = false;
      KeyDecode.arrowLeft = false;
      KeyDecode.arrowRight = false;
      KeyDecode.delete = false;

      if (key == Key.Back)
      {
        KeyDecode.backspace = true;
        KeyDecode.printable = false;
        return;
      }

      if (key == Key.Delete)
      {
        KeyDecode.delete = true;
        KeyDecode.printable = false;
        return;
      }

      if (key == Key.Left)
      {
        KeyDecode.arrowLeft = true;
        KeyDecode.printable = false;
        return;
      }

      if (key == Key.Right)
      {
        KeyDecode.arrowRight = true;
        KeyDecode.printable = false;
        return;
      }

      switch (key)
      {
        case Key.Enter: KeyDecode.character = '\n'; return;
        case Key.A: KeyDecode.character = (iscap ? 'A' : 'a'); return;
        case Key.B: KeyDecode.character = (iscap ? 'B' : 'b'); return;
        case Key.C: KeyDecode.character = (iscap ? 'C' : 'c'); return;
        case Key.D: KeyDecode.character = (iscap ? 'D' : 'd'); return;
        case Key.E: KeyDecode.character = (iscap ? 'E' : 'e'); return;
        case Key.F: KeyDecode.character = (iscap ? 'F' : 'f'); return;
        case Key.G: KeyDecode.character = (iscap ? 'G' : 'g'); return;
        case Key.H: KeyDecode.character = (iscap ? 'H' : 'h'); return;
        case Key.I: KeyDecode.character = (iscap ? 'I' : 'i'); return;
        case Key.J: KeyDecode.character = (iscap ? 'J' : 'j'); return;
        case Key.K: KeyDecode.character = (iscap ? 'K' : 'k'); return;
        case Key.L: KeyDecode.character = (iscap ? 'L' : 'l'); return;
        case Key.M: KeyDecode.character = (iscap ? 'M' : 'm'); return;
        case Key.N: KeyDecode.character = (iscap ? 'N' : 'n'); return;
        case Key.O: KeyDecode.character = (iscap ? 'O' : 'o'); return;
        case Key.P: KeyDecode.character = (iscap ? 'P' : 'p'); return;
        case Key.Q: KeyDecode.character = (iscap ? 'Q' : 'q'); return;
        case Key.R: KeyDecode.character = (iscap ? 'R' : 'r'); return;
        case Key.S: KeyDecode.character = (iscap ? 'S' : 's'); return;
        case Key.T: KeyDecode.character = (iscap ? 'T' : 't'); return;
        case Key.U: KeyDecode.character = (iscap ? 'U' : 'u'); return;
        case Key.V: KeyDecode.character = (iscap ? 'V' : 'v'); return;
        case Key.W: KeyDecode.character = (iscap ? 'W' : 'w'); return;
        case Key.X: KeyDecode.character = (iscap ? 'X' : 'x'); return;
        case Key.Y: KeyDecode.character = (iscap ? 'Y' : 'y'); return;
        case Key.Z: KeyDecode.character = (iscap ? 'Z' : 'z'); return;
        case Key.D0: KeyDecode.character = (shift ? ')' : '0'); return;
        case Key.D1: KeyDecode.character = (shift ? '!' : '1'); return;
        case Key.D2: KeyDecode.character = (shift ? '@' : '2'); return;
        case Key.D3: KeyDecode.character = (shift ? '#' : '3'); return;
        case Key.D4: KeyDecode.character = (shift ? '$' : '4'); return;
        case Key.D5: KeyDecode.character = (shift ? '%' : '5'); return;
        case Key.D6: KeyDecode.character = (shift ? '^' : '6'); return;
        case Key.D7: KeyDecode.character = (shift ? '&' : '7'); return;
        case Key.D8: KeyDecode.character = (shift ? '*' : '8'); return;
        case Key.D9: KeyDecode.character = (shift ? '(' : '9'); return;
        case Key.OemPlus: KeyDecode.character = (shift ? '+' : '='); return;
        case Key.OemMinus: KeyDecode.character = (shift ? '_' : '-'); return;
        case Key.OemQuestion: KeyDecode.character = (shift ? '?' : '/'); return;
        case Key.OemComma: KeyDecode.character = (shift ? '<' : ','); return;
        case Key.OemPeriod: KeyDecode.character = (shift ? '>' : '.'); return;
        case Key.OemOpenBrackets: KeyDecode.character = (shift ? '{' : '['); return;
        case Key.OemQuotes: KeyDecode.character = (shift ? '"' : '\''); return;
        case Key.Oem1: KeyDecode.character = (shift ? ':' : ';'); return;
        case Key.Oem3: KeyDecode.character = (shift ? '~' : '`'); return;
        case Key.Oem5: KeyDecode.character = (shift ? '|' : '\\'); return;
        case Key.Oem6: KeyDecode.character = (shift ? '}' : ']'); return;
        case Key.Tab: KeyDecode.character = '\t'; return;
        case Key.Space: KeyDecode.character = ' '; return;

        // Number Pad
        case Key.NumPad0: KeyDecode.character = '0'; return;
        case Key.NumPad1: KeyDecode.character = '1'; return;
        case Key.NumPad2: KeyDecode.character = '2'; return;
        case Key.NumPad3: KeyDecode.character = '3'; return;
        case Key.NumPad4: KeyDecode.character = '4'; return;
        case Key.NumPad5: KeyDecode.character = '5'; return;
        case Key.NumPad6: KeyDecode.character = '6'; return;
        case Key.NumPad7: KeyDecode.character = '7'; return;
        case Key.NumPad8: KeyDecode.character = '8'; return;
        case Key.NumPad9: KeyDecode.character = '9'; return;
        case Key.Subtract: KeyDecode.character = '-'; return;
        case Key.Add: KeyDecode.character = '+'; return;
        case Key.Decimal: KeyDecode.character = '.'; return;
        case Key.Divide: KeyDecode.character = '/'; return;
        case Key.Multiply: KeyDecode.character = '*'; return;

        

        default:
          KeyDecode.type = 1;
          KeyDecode.printable = false;
          KeyDecode.character = '\x00';
          return;
      } //switch          
    } // function
  }

  public struct IoCmd_t
  {
    public Key key;
    public bool printable;
    public char character;
    public bool shift;
    public bool ctrl;
    public bool alt;
    public bool backspace;
    public bool delete;
    public bool arrowLeft;
    public bool arrowRight;
    public int type; //sideband
    public string s;    //sideband
  };
}

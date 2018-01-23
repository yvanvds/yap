using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YapView
{
  public class Object
  {
    public IYapHandler Handler { get; set; }
    public object handle = null;
    string currentObjectName = "";


    SKRect rect;
    SKSize size;

    List<SKRect> input = new List<SKRect>();
    public int Inputs
    {
      get => input.Capacity;
      set
      {
        input = new List<SKRect>(value);
        UpdateLayout();
      }
    }

    List<SKRect> output = new List<SKRect>();
    public int Outputs
    {
      get => output.Capacity;
      set
      {
        output = new List<SKRect>(value);
        UpdateLayout();
      }
    }


    String text = "";
    public String Text
    {
      get => text;
      set
      {
        text = value;
        float width = 8 + Paint.Text.MeasureText(Text);
        if (width < 80) width = 80;
        size.Width = width;
        UpdateLayout();
      }
    }

    public bool Selected { get; set; }
    public bool EditMode { get; set; }

    public bool HasChanged()
    {
      return !Text.Equals(currentObjectName);
    }

    public bool Reconfigure() {
      // separate object name from arguments
      string[] original = currentObjectName.Split(' ');
      string[] newText = Text.Split(' ');
      currentObjectName = Text;
      bool objectIsReplaced = false;

      if (handle != null)
      {
        if(newText.Length > 0)
        {
          // reconstruct object if name is different
          if(!original[0].Equals(newText[0]))
          {
            Handler.DeleteObject(handle);
            handle = Handler.CreateObject(Text);
            objectIsReplaced = true;
          }
        } else
        {
          Handler.DeleteObject(handle);
          handle = null;
          objectIsReplaced = true;
        }
      } else
      {
        // handle is null, create a new object if possible
        if(newText.Length > 0)
        {
          handle = Handler.CreateObject(newText[0]);
          objectIsReplaced = true;
        }
      }

      if (handle != null)
      {
        // pass arguments
        var args = currentObjectName.Split(new[] { ' ' }, 2);
        if(args.Length > 1)
        {
          Handler.PassArgument(handle, args[1]);
        } else
        {
          Handler.PassArgument(handle, "");
        }
        
        // set inlets and outlets
        Inputs = Handler.GetObjectInputCount(handle);
        Outputs = Handler.GetObjectOutputCount(handle);
      } else
      {
        Inputs = 0;
        Outputs = 0;
      }
      UpdateLayout();

      return objectIsReplaced;
    }

    public bool Hover { get; set; }

    public int CarretPos { get; set; } = 0;

    public Object(SKPoint pos, IYapHandler handler)
    {
      Handler = handler;
      rect = new SKRect();
      size = new SKSize(100f, 25f);

      rect.Left = (float)pos.X;
      rect.Top = (float)pos.Y;
      rect.Size = size;

      Inputs = 0;
      Outputs = 0;
      Selected = true;
      EditMode = true;

      CarretPos = 0;
    }

    public bool IsInside(SKPoint pos)
    {
      if(pos.X > rect.Left && pos.X < rect.Left + rect.Width)
      {
        if(pos.Y > rect.Top && pos.Y < rect.Top + rect.Height)
        {
          return true;
        }
      }
      return false;
    }

    public int OnInput(SKPoint pos)
    {
      for(int i = 0; i < input.Count; i++)
      {
        if(pos.X >= input[i].Left - 5 && pos.X <= input[i].Left + input[i].Width + 5)
        {
          if(pos.Y >= input[i].Top - 5 && pos.Y <= input[i].Top + input[i].Height + 5)
          {
            return i;
          }
        }
      }
      return -1;
    }

    public int OnOutput(SKPoint pos)
    {
      for (int i = 0; i < output.Count; i++)
      {
        if (pos.X >= output[i].Left && pos.X <= output[i].Left + output[i].Width)
        {
          if (pos.Y >= output[i].Top && pos.Y <= output[i].Top + output[i].Height)
          {
            return i;
          }
        }
      }
      return -1;
    }

    public int FindCarretPos(SKPoint pos)
    {
      string s = Text;
      float length = Paint.Text.MeasureText(s);
      while(s.Length > 0 && pos.X < rect.Left + 4 + length)
      {
        s = s.Remove(s.Length - 1);
        length = Paint.Text.MeasureText(s);
      }
      return s.Length;
    }

    public void Draw(SKCanvas canvas)
    {
      
      if(Selected)
      {
        canvas.DrawRect(rect, Paint.ObjectFillSelected);
        canvas.DrawRect(rect, Paint.ObjectBorderSelected);
      }
      else if(Hover)
      {
        canvas.DrawRect(rect, Paint.ObjectFill);
        canvas.DrawRect(rect, Paint.ObjectBorderHovered);
      }
      else
      {
        canvas.DrawRect(rect, Paint.ObjectFill);
        canvas.DrawRect(rect, Paint.ObjectBorder);
      }

      canvas.DrawLine(rect.Left, rect.Top + 5, rect.Right, rect.Top + 5, Paint.ObjectBorder);
      canvas.DrawLine(rect.Left, rect.Bottom - 5, rect.Right, rect.Bottom - 5, Paint.ObjectBorder);

      foreach(var rect in input)
      {
        canvas.DrawRect(rect, Paint.Pin);
      }

      foreach(var rect in output)
      {
        canvas.DrawRect(rect, Paint.Pin);
      }

      canvas.DrawText(Text, rect.Left + 4, rect.Top + 17, Paint.Text);

      if(EditMode)
      {
        string s = Text;
        if (CarretPos < Text.Length) s = s.Remove(CarretPos);
        float xPos = rect.Left + 4 + Paint.Text.MeasureText(s);
        canvas.DrawLine(xPos, rect.Top + 5, xPos, rect.Bottom - 4, Paint.Carret);
      }
    }

    public void Move(SKPoint delta)
    {
      rect.Left = rect.Left + delta.X;
      rect.Top = rect.Top + delta.Y;
      rect.Size = size;
      UpdateLayout();
    }

    public SKPoint GetOutputPos(int pin)
    {
      SKPoint p = new SKPoint();
      if (pin < output.Count)
      {
        p.X = output[pin].MidX;
        p.Y = output[pin].MidY;
      }
      return p;
    }

    public SKPoint GetInputPos(int pin)
    {
      SKPoint p = new SKPoint();
      if (pin < input.Count)
      {
        p.X = input[pin].MidX;
        p.Y = input[pin].MidY;
      }
      return p;
    }

    private void UpdateLayout()
    { 
      rect.Size = size;

      {
        // calculate inputs
        float inputWidth = 10;
        float inputHeight = 5;
        float blankSpace = 0;
        float left = rect.Left;
        float top = rect.Top;


        if (Inputs > 1)
        {
          blankSpace = ((size.Width + 1) - (inputWidth * Inputs)) / (Inputs - 1);
        }
        input.Clear();
        for (int i = 0; i < input.Capacity; i++)
        {
          SKRect r = new SKRect();
          r.Left = left;
          r.Top = top;
          r.Size = new SKSize(inputWidth, inputHeight);
          input.Add(r);

          left += inputWidth + blankSpace;
        }
      }

      {
        // calculate outputs
        float outputWidth = 10;
        float outputHeight = 5;
        float blankSpace = 0;
        float left = rect.Left;
        float top = rect.Bottom - 5;


        if (Outputs > 1)
        {
          blankSpace = (size.Width - (outputWidth * Outputs)) / (Outputs - 1);
        }
        output.Clear();
        for (int i = 0; i < output.Capacity; i++)
        {
          SKRect r = new SKRect();
          r.Left = left;
          r.Top = top;
          r.Size = new SKSize(outputWidth, outputHeight);
          output.Add(r);

          left += outputWidth + blankSpace;
        }
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace YapView.Gui
{
  internal class ConnectEnabled : IConnect
  {
    List<SKRect> input = new List<SKRect>();
    public uint Inputs
    {
      get => (uint)input.Capacity;
      set
      {
        input = new List<SKRect>((int)value);
      }
    }

    List<SKRect> output = new List<SKRect>();
    public uint Outputs
    {
      get => (uint)output.Capacity;
      set
      {
        output = new List<SKRect>((int)value);
      }
    }

    public void Draw(SKRect rect, SKCanvas canvas)
    {
      canvas.DrawLine(rect.Left, rect.Top + 2, rect.Right, rect.Top + 2, Paint.ObjectBorder);
      canvas.DrawLine(rect.Left, rect.Bottom - 2, rect.Right, rect.Bottom - 2, Paint.ObjectBorder);

      foreach (var i in input)
      {
        SKRect drawRect = new SKRect(
          i.Left + 2,
          i.Top - 4,
          i.Right - 2,
          i.Bottom - 2
          );

        using (SKPath path = new SKPath())
        {
          path.AddArc(drawRect, 0, 180);
          canvas.DrawPath(path, Paint.Pin);
        }
        //canvas.DrawRect(rect, Paint.Pin);
      }

      foreach (var o in output)
      {
        SKRect drawRect = new SKRect(
          o.Left + 2,
          o.Top + 2,
          o.Right - 2,
          o.Bottom + 2
          );
        using (SKPath path = new SKPath())
        {
          path.AddArc(drawRect, 180, 180);
          canvas.DrawPath(path, Paint.Pin);
        }
      }
    }

    public SKPoint GetInputPos(uint inlet)
    {
      SKPoint p = new SKPoint();
      if (inlet < input.Count)
      {
        p.X = input[(int)inlet].MidX;
        p.Y = input[(int)inlet].MidY;
      }
      return p;
    }

    public SKPoint GetOutputPos(uint outlet)
    {
      SKPoint p = new SKPoint();
      if (outlet < output.Count)
      {
        p.X = output[(int)outlet].MidX;
        p.Y = output[(int)outlet].MidY;
      }
      return p;
    }

    public int OnInput(SKPoint pos)
    {
      for (int i = 0; i < input.Count; i++)
      {
        SKRect rect = new SKRect(input[i].Left - 3, input[i].Top - 3, input[i].Right + 3, input[i].Bottom + 3);
        if (rect.Contains(pos)) return i;
      }
      return -1;
    }

    public int OnOutput(SKPoint pos)
    {
      for (int i = 0; i < output.Count; i++)
      {
        if (output[i].Contains(pos)) return i;
      }
      return -1;
    }

    public void Update(SKRect rect)
    {
      {
        // calculate inputs
        float inputWidth = 10;
        float inputHeight = 5;
        float blankSpace = 0;
        float left = rect.Left;
        float top = rect.Top;


        if (Inputs > 1)
        {
          blankSpace = ((rect.Width + 1) - (inputWidth * Inputs)) / (Inputs - 1);
        }
        input.Clear();
        for (int i = 0; i < input.Capacity; i++)
        {
          SKRect r = new SKRect
          {
            Left = left,
            Top = top,
            Size = new SKSize(inputWidth, inputHeight)
          };
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
          blankSpace = (rect.Width - (outputWidth * Outputs)) / (Outputs - 1);
        }
        output.Clear();
        for (int i = 0; i < output.Capacity; i++)
        {
          SKRect r = new SKRect
          {
            Left = left,
            Top = top,
            Size = new SKSize(outputWidth, outputHeight)
          };
          output.Add(r);

          left += outputWidth + blankSpace;
        }
      }
    }
  }
}

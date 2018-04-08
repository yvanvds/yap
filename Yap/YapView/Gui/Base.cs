using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YapView.Gui
{
  public abstract class Base
  {
    protected Object obj;
    protected SKRect rect;
    protected SKSize size;
    protected SKPaint font = Paint.Text;

    public SKRect Rect { get => rect; }

    protected List<SKRect> input = new List<SKRect>();
    public int Inputs
    {
      get => input.Capacity;
      set
      {
        input = new List<SKRect>(value);
        Update();
      }
    }

    protected List<SKRect> output = new List<SKRect>();
    public int Outputs
    {
      get => output.Capacity;
      set
      {
        output = new List<SKRect>(value);
        Update();
      }
    }

    String text = "";
    public String Text
    {
      get => text;
      set
      {
        text = value;
        if (hasFixedSize) return;

        float width = 8 + font.MeasureText(Text);
        if (width < 80) width = 80;
        size.Width = width;
        Update();
      }
    }

    protected bool hasFixedSize = false;
    protected bool hasEditableGui = false;
    public bool HasEditableGui { get => hasEditableGui; }
    public bool Hover { get; set; }
    public bool Selected { get; set; }
    public bool EditMode { get; set; }
    public bool GuiEditMode { get; set; }
    public EditModeStyle GuiEditModeStyle = EditModeStyle.Line;

    protected string guiValue = "";
    public string GuiValue { get => guiValue; set => SendGuiValue(value); }

    protected virtual void SendGuiValue(string value) { }

    public int CarretPos { get; set; } = 0;


    public Base(SKPoint pos, Object obj)
    {
      this.obj = obj;
      rect = new SKRect();
      rect.Left = pos.X;
      rect.Top = pos.Y;

      Inputs = 0;
      Outputs = 0;

      EditMode = true;
      Selected = true;
    }

    public void Move(SKPoint delta)
    {
      rect.Left = rect.Left + delta.X;
      rect.Top = rect.Top + delta.Y;
      rect.Size = size;
      obj.StorePosition();
      Update();
    }

    public bool IsInside(SKPoint pos)
    {
      if (pos.X > rect.Left && pos.X < rect.Left + rect.Width)
      {
        if (pos.Y > rect.Top && pos.Y < rect.Top + rect.Height)
        {
          return true;
        }
      }
      return false;
    }

    public int OnInput(SKPoint pos)
    {
      for (int i = 0; i < input.Count; i++)
      {
        if (pos.X >= input[i].Left - 5 && pos.X <= input[i].Left + input[i].Width + 5)
        {
          if (pos.Y >= input[i].Top - 5 && pos.Y <= input[i].Top + input[i].Height + 5)
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

    public int SetCarretPos(SKPoint pos)
    {
      string s = Text;
      if (GuiEditMode) s = guiValue;

      float length = font.MeasureText(s);
      while (s.Length > 0 && pos.X < rect.Left + 4 + length)
      {
        s = s.Remove(s.Length - 1);
        length = font.MeasureText(s);
      }
      CarretPos = s.Length;
      return CarretPos;
    }

    public virtual bool OnMouseDown(SKPoint mousePos)
    {
      if (obj.View.PerformanceMode)
      {
        if(HasEditableGui)
        {
          if(Selected)
          {
            GuiEditMode = true;
            obj.Editor.Edit(GuiValue, SetCarretPos(mousePos), GuiEditModeStyle);
            return true; // event is handled
          } else
          {
            // select, next click will trigger editor
            Selected = true;
            return true; // event is handled
          }
        } else
        {
          // this control has no editor
          return false; // child should handle event
        }
      } else
      {
        // not in performance mode
        if(Selected)
        {
          EditMode = true;
          obj.Editor.Edit(Text, SetCarretPos(mousePos), EditModeStyle.Line);
        } else
        {
          Selected = true;
        }
      }
      return true;
    }

    public virtual void OnMouseMove(SKPoint mousePos) { }
    public virtual void OnMouseUp(SKPoint mousePos) { }
    public virtual void OnMouseWheel(MouseWheelEventArgs e, float posX) { }

    public abstract void UpdateGui();

    public void Update()
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

      UpdateGui();
    }

    public void Draw(SKCanvas canvas)
    {
      if(EditMode || GuiEditMode)
      {
        DrawEditMode(canvas);
      } else
      {
        DrawGui(canvas);
      }
    }

    public abstract void DrawGui(SKCanvas canvas);
    public virtual void DrawGuiEdit(SKCanvas canvas) { }

    public void DrawEditMode(SKCanvas canvas)
    {
      canvas.DrawRect(rect, Paint.ObjectFillSelected);

      DrawPins(canvas);

      if(EditMode)
      {
        canvas.DrawText(Text, rect.Left + 4, rect.Top + 17, Paint.Text);
        string s = Text;
        if (CarretPos < Text.Length) s = s.Remove(CarretPos);
        float xPos = rect.Left + 4 + font.MeasureText(s);
        canvas.DrawLine(xPos, rect.Top + 5, xPos, rect.Bottom - 4, Paint.Carret);
      }
      else if (GuiEditMode)
      {
        DrawGuiEdit(canvas);
      }
    }

    public void DrawPins(SKCanvas canvas)
    {
      if (obj.View.PerformanceMode) return;

      canvas.DrawLine(rect.Left, rect.Top + 2, rect.Right, rect.Top + 2, Paint.ObjectBorder);
      canvas.DrawLine(rect.Left, rect.Bottom - 2, rect.Right, rect.Bottom - 2, Paint.ObjectBorder);

      foreach (var rect in input)
      {
        SKRect drawRect = new SKRect(
          rect.Left + 2,
          rect.Top - 4,
          rect.Right - 2,
          rect.Bottom - 2
          );

        using (SKPath path = new SKPath())
        {
          path.AddArc(drawRect, 0, 180);
          canvas.DrawPath(path, Paint.Pin);
        }
        //canvas.DrawRect(rect, Paint.Pin);
      }

      foreach (var rect in output)
      {
        SKRect drawRect = new SKRect(
          rect.Left + 2,
          rect.Top + 2,
          rect.Right - 2,
          rect.Bottom + 2
          );
        using (SKPath path = new SKPath())
        {
          path.AddArc(drawRect, 180, 180);
          canvas.DrawPath(path, Paint.Pin);
        }
      }
    }
  }
}

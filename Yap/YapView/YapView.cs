using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace YapView
{
  /// <summary>
  /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
  ///
  /// Step 1a) Using this custom control in a XAML file that exists in the current project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:YapView"
  ///
  ///
  /// Step 1b) Using this custom control in a XAML file that exists in a different project.
  /// Add this XmlNamespace attribute to the root element of the markup file where it is 
  /// to be used:
  ///
  ///     xmlns:MyNamespace="clr-namespace:YapView;assembly=YapView"
  ///
  /// You will also need to add a project reference from the project where the XAML file lives
  /// to this project and Rebuild to avoid compilation errors:
  ///
  ///     Right click on the target project in the Solution Explorer and
  ///     "Add Reference"->"Projects"->[Select this project]
  ///
  ///
  /// Step 2)
  /// Go ahead and use your control in the XAML file.
  ///
  ///     <MyNamespace:CustomControl1/>
  ///
  /// </summary>
  public class YapView : SkiaSharp.Views.WPF.SKElement
  {
    static YapView()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(YapView), new FrameworkPropertyMetadata(typeof(YapView)));
    }

    SKPaint bluePaint = new SKPaint()
    {
      Style = SKPaintStyle.Fill,
      Color = SKColors.Blue
    };

    Object o;
    Object current = null;

    float Width = 0f;
    float Height = 0f;

    StringEditor SEdit = new StringEditor();

    public YapView()
    {
      o = new Object(100f, 100f);
      o.Text = "Hello is a bit short";
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
      SKSurface surface = e.Surface;
      SKCanvas canvas = surface.Canvas;

      Width = e.Info.Width;
      Height = e.Info.Height;

      canvas.Clear(SKColors.DimGray);

      o.Draw(canvas);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      Point pos = e.GetPosition(this);
      pos.X = pos.X / RenderSize.Width * Width;
      pos.Y = pos.Y / RenderSize.Height * Height;
      if(o.IsInside(pos))
      {
        if(o.Selected)
        {
          o.EditMode = true;
          SEdit.Edit(current.Text, 0);
        } else
        {
          o.Selected = true;
        }
        current = o;
        InvalidateVisual();
      } else
      {
        o.EditMode = false;
        o.Selected = false;
        current = null;
        InvalidateVisual();
      }
      e.Handled = true;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if(current != null && current.EditMode == true)
      {
        current.Text = SEdit.Update(e.Key);
        InvalidateVisual();
      }
    }
  }
}

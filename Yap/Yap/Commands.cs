using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Yap.Commands
{
  public static class AppCommands
  {
    public static readonly RoutedUICommand Exit = new RoutedUICommand
    (
      "Exit",
      "Exit",
      typeof(AppCommands),
      new InputGestureCollection()
      {
        new KeyGesture(Key.F4, ModifierKeys.Alt)
      }
    );

    public static readonly RoutedUICommand AddObject = new RoutedUICommand
    (
      "Add Object",
      "AddObject",
      typeof(AppCommands)
    );

    public static readonly RoutedUICommand Perform = new RoutedUICommand
    (
      "Perform",
      "Perform",
      typeof(AppCommands)
    );

    public static readonly RoutedUICommand Help = new RoutedUICommand
    (
      "Help",
      "Help",
      typeof(AppCommands)
    );

    public static readonly RoutedUICommand About = new RoutedUICommand
    (
      "About",
      "About",
      typeof(AppCommands)
    );
  }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace Yap
{
  /// <summary>
  /// Interaction logic for YapWindow.xaml
  /// </summary>
  public partial class YapWindow : Window
  {
    YSE.ISound sound;
    YSE.IPatcher patcher;

    String currentFileName = "";

    public YapWindow()
    {
      InitializeComponent();

      sound = Global.YseObj.CreateSound();
      patcher = Global.YseObj.CreatePatcher();
      patcher.Create(1);

      yap.Handle = new YapHandler(patcher);
      yap.Focusable = true;
      yap.Focus();
      yap.Init();

      sound.Create(patcher);
      sound.Play();

      Title = "new patcher";
    }

    protected override void OnClosed(EventArgs e)
    {
      yap.Clear();
      base.OnClosed(e);

      patcher.Dispose();
      sound.Dispose();
    }

    private void AddObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      string parameter = (string)e.Parameter;
      if (parameter.Equals("key", StringComparison.CurrentCultureIgnoreCase))
      {
        yap.AddObject(true); // use mouse position
      }
      else
      {
        yap.AddObject(false);
      }

    }

    private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Yap File (*.yap)|*.yap";
      if (dialog.ShowDialog() == true)
      {
        string content = File.ReadAllText(dialog.FileName);

        YapWindow window = new YapWindow();
        window.Show();
        window.LoadContent(dialog.FileName, content);
      }
    }

    public void LoadContent(string filename, string content)
    {
      yap.Clear();
      yap.Load(content);
      currentFileName = filename;
      Title = currentFileName;
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = currentFileName.Length > 0;
    }

    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      string content = yap.Save();
      File.WriteAllText(currentFileName, content);
    }

    private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Filter = "Yap File (*.yap)|*.yap";
      if (dialog.ShowDialog() == true)
      {
        string content = yap.Save();
        File.WriteAllText(dialog.FileName, content);
        currentFileName = dialog.FileName;

        Title = currentFileName;
      } 
    }

    private void AddObjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void PerformCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void PerformCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      yap.Deselect();
      YapView.Interface.PerformanceMode = !YapView.Interface.PerformanceMode;
      Perform.IsChecked = YapView.Interface.PerformanceMode;
    }

    private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      YapWindow window = new YapWindow();
      window.Show();
    }
  }
}

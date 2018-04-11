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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Yap.Commands;

namespace Yap
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    YSE.IGlobal YseObj = new YSENET.Global();
    YapHandler Handler;
    DispatcherTimer UpdateYSE = new DispatcherTimer();

    YSE.ISound sound;
    YSE.IPatcher patcher;

    String currentFileName = "";

    public MainWindow()
    {
      InitializeComponent();
      YseObj.Log.Level = YSE.ERROR_LEVEL.DEBUG;
      YseObj.Log.OnMessage += OnMessage;

      YseObj.System.Init();
      UpdateYSE.Interval = new TimeSpan(0, 0, 0, 0, 50);
      UpdateYSE.Tick += new EventHandler(Update);
      UpdateYSE.Start();

      sound = YseObj.CreateSound();
      patcher = YseObj.CreatePatcher();
      patcher.Create(1);
      YapView.Interface.Handle = new YapHandler(patcher);

      sound.Create(patcher);
      sound.Play();
      //Yse.Yse.System().AudioTest(true);

      yap.Focusable = true;
      yap.Focus();
      yap.Init();

      
    }

    private void Update(object sender, EventArgs e)
    {
      Yse.Yse.System().update();
    }

    protected override void OnClosed(EventArgs e)
    {
      yap.Clear();
      UpdateYSE.Stop();
      Yse.Yse.System().close();
      base.OnClosed(e);

      patcher.Dispose();
      sound.Dispose();
    }

    private void OnMessage(string message)
    {
      log.Text = message + Environment.NewLine + log.Text;
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }

    private void AddObjectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      string parameter = (string)e.Parameter;
      if(parameter.Equals("key", StringComparison.CurrentCultureIgnoreCase))
      {
        yap.AddObject(true); // use mouse position
      } else
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
        yap.Clear();
        string content = File.ReadAllText(dialog.FileName);
        yap.Load(content);
        currentFileName = dialog.FileName;
      }
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
      } 
    }

    private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
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
  }
}

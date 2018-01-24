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
      Handler = new YapHandler(patcher);

      sound.Create(patcher);
      sound.Play();
      //Yse.Yse.System().AudioTest(true);

      yap.Focusable = true;
      yap.Focus();
      yap.Init(Handler);

      
    }

    private void Update(object sender, EventArgs e)
    {
      Yse.Yse.System().update();
    }

    protected override void OnClosed(EventArgs e)
    {
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

    private void Load_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Yap File (*.yap)|*.yap";
      if(dialog.ShowDialog() == true)
      {
        string content = File.ReadAllText(dialog.FileName);
        yap.Load(content);
      }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Filter = "Yap File (*.yap)|*.yap";
      if(dialog.ShowDialog() == true)
      {
        string content = yap.Save();
        File.WriteAllText(dialog.FileName, content);
      }
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
  }
}

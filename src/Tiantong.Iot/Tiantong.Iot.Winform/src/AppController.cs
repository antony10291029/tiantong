using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiantong.Iot.Winforms
{
  public class AppController
  {
    private Config _config;

    private IHost _server;

    private string _runningPort;

    private RootForm _rootForm;

    private NotifyIcon _notifyIcon;

    private ContextMenuStrip _menuStrip;

    public AppController()
    {
      _config = new Config();
      InitializeForms();
      Task.Run(StartServer);
      InitializeMenuStrip();
      InitializeNotifyIcon();
      _rootForm._logTextBox.Text = $"{DateTime.Now}:  通信系统服务自动开始运行\r\n" + _rootForm._logTextBox.Text;
    }

    public void Dispose()
    {
      DisposeMenuStrip();
      DisposeNotifyIcon();
      DisposeForms();
    }

    private void InitializeForms()
    {
      _rootForm = new RootForm();
      _rootForm._exitButton.Click += async (sender, args) => await HandleExit();
      _rootForm._startButton.Click += async (sender, args) => {
        await StartServer();
        _rootForm._stopButton.Visible = true;
        _rootForm._startButton.Visible = false;
        _rootForm._logTextBox.Text = $"{DateTime.Now}:  通信系统服务已启动\r\n" + _rootForm._logTextBox.Text;
      };
      _rootForm._stopButton.Click += async (sender, args) => {
        await StopServer();
        _rootForm._stopButton.Visible = false;
        _rootForm._startButton.Visible = true;
        _rootForm._logTextBox.Text = $"{DateTime.Now}:  通信系统服务已停止\r\n" + _rootForm._logTextBox.Text;
      };
      _rootForm._openClientButton.Click += (sender, args) => {
        if (_runningPort == null) {
          MessageBox.Show("检测到通信系统未运行，请先启动服务", "提示", MessageBoxButtons.OK);
          return;
        }

        var ps = new ProcessStartInfo($"http://localhost:{_runningPort}") {
          UseShellExecute = true,
        };

        Process.Start(ps);
      };
      _rootForm._portTextBox.Text = _config.Port;
      _rootForm._portTextBox.Leave += (sender, args) => {
        _config.Port = _rootForm._portTextBox.Text;
      };
      _rootForm.Show();
      BootAutostartCheckbox();
    }

    private void DisposeForms()
    {
      _rootForm.Dispose();
    }

    private void InitializeMenuStrip()
    {
      _menuStrip = new ContextMenuStrip();
      _menuStrip.Items.Add("Exit", null, async (sender, args) => await HandleExit());
    }

    private void DisposeMenuStrip()
    {
      _menuStrip.Dispose();
    }

    private void InitializeNotifyIcon()
    {
      _notifyIcon = new NotifyIcon();
      _notifyIcon.Visible = true;
      _notifyIcon.Text = "天瞳IOT - 服务管理";
      _notifyIcon.Icon = new Icon("icon.ico", 30, 30);
      _notifyIcon.ContextMenuStrip = _menuStrip;
      _notifyIcon.MouseDoubleClick += (sender, args) => {
        if (_rootForm.Visible) {
          _rootForm.Hide();
        } else {
          _rootForm.Show();
        }
      };
    }

    private void BootAutostartCheckbox()
    {
      var registerKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

      _rootForm._autorunCheckbox.Checked = registerKey.GetValue("TiantongIot") != null;
      _rootForm._autorunCheckbox.CheckedChanged += HandleAutorunCheckboxChanged;
      registerKey.Close();
    }

    private void HandleAutorunCheckboxChanged(object sender, EventArgs e)
    {
      var enabled = _rootForm._autorunCheckbox.Checked;
      var value = Directory.GetCurrentDirectory() + $@"\{Application.ProductName}.exe";
      var registerKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

      if (enabled) {
        registerKey.SetValue("TiantongIot", value);
      } else {
        registerKey.DeleteValue("TiantongIot", true);
      }

      MessageBox.Show(value);
      registerKey.Close();
    }

    private void DisposeNotifyIcon()
    {
      _notifyIcon.Dispose();
    }

    private async Task StartServer()
    {
      if (_server != null) {
        return;
      }

      _runningPort = _rootForm._portTextBox.Text;

      _server = Tiantong.Iot.Api.Program.CreateHostBuilder(new string[] {}, _runningPort).Build();

      await _server.StartAsync();
    }

    private async Task StopServer()
    {
      await _server?.StopAsync();
      await _server?.WaitForShutdownAsync();

      _server?.Dispose();
      _server = null;
      _runningPort = null;
    }

    private async Task HandleExit()
    {
      var result = MessageBox.Show("退出后通信系统将停止运行。", "确认退出", MessageBoxButtons.YesNo);

      if (result == DialogResult.Yes) {
        if (_server != null) {
          await StopServer();
        }

        Dispose();
        Application.Exit();
      }
    }
  }
}

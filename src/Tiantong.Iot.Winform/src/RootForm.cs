using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Tiantong.Iot.Winforms
{
  public partial class RootForm : Form
  {
    private IContainer _components;

    public Button _openClientButton;

    public Button _startButton;

    public Button _stopButton;

    public Button _exitButton;

    public TextBox _logTextBox;

    public Label _portLabel;

    public TextBox _portTextBox;

    public RootForm()
    {
      InitializeOpenClientButton();
      InitializeStartServerButton();
      InitializeStopServerButton();
      InitializeCloseAndExitButton();
      InitializeLogTextBox();
      InitializePortLabel();
      InitializePortTextBox();
      InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      DisposeComponent(disposing);
      base.Dispose(disposing);
    }

    private void InitializeOpenClientButton()
    {
      _openClientButton = new Button();
      _openClientButton.Text = "打开客户端";
      _openClientButton.Size = new Size(160, 52);
      _openClientButton.Location = new Point(60, 500);
      _openClientButton.Visible = true;
      Controls.Add(_openClientButton);
    }

    private void InitializeStartServerButton()
    {
      _startButton = new Button();
      _startButton.Text = "启动服务";
      _startButton.Size = new Size(160, 52);
      _startButton.Location = new Point(280, 500);
      _startButton.Visible = false;
      Controls.Add(_startButton);
    }

    private void InitializeStopServerButton()
    {
      _stopButton = new Button();
      _stopButton.Text = "停止服务";
      _stopButton.Size = new Size(160, 52);
      _stopButton.Location = new Point(280, 500);
      _stopButton.Visible = true;
      Controls.Add(_stopButton);
    }

    private void InitializeCloseAndExitButton()
    {
      _exitButton = new Button();
      _exitButton.Text = "关闭退出";
      _exitButton.Size = new Size(160, 52);
      _exitButton.Location = new Point(500, 500);
      _exitButton.Visible = true;
      Controls.Add(_exitButton);
    }

    private void InitializeLogTextBox()
    {
      _logTextBox = new TextBox();
      _logTextBox.Text = "";
      _logTextBox.ReadOnly = true;
      _logTextBox.Multiline = true;
      _logTextBox.Size = new Size(420, 420);
      _logTextBox.Location = new Point(60, 40);
      _logTextBox.Visible = true;
      Controls.Add(_logTextBox);
    }

    private void InitializePortLabel()
    {
      _portLabel = new Label();
      _portLabel.Text = "服务端口";
      _portLabel.Size = new Size(84, 40);
      _portLabel.Location = new Point(492, 42);
      _portLabel.Visible = true;
      Controls.Add(_portLabel);
    }

    private void InitializePortTextBox()
    {
      _portTextBox = new TextBox();
      _portTextBox.Size = new Size(80, 40);
      _portTextBox.Location = new Point(580, 40);
      _portTextBox.Visible = true;
      Controls.Add(_portTextBox);
    }

    private void InitializeComponent()
    {
      _components = new Container();
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new Size(720, 600);
      Text = "天瞳物联网系统 - 启动程序";
      MaximizeBox = false;
      MinimizeBox = false;
      FormBorderStyle = FormBorderStyle.FixedSingle;
      CenterToScreen();
    }

    private void DisposeComponent(bool disposing)
    {
      if (disposing && (_components != null))
      {
        _components.Dispose();
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Hide();

      e.Cancel = true;
    }

  }
}

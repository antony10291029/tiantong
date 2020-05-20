using System.Diagnostics;
using System.Windows.Forms;

namespace Tiantong.Iot.Winforms
{
  static class Program
  {
    static void Main()
    {
      if (IsProcessExisted()) {
        return;
      }

      Application.SetHighDpiMode(HighDpiMode.SystemAware);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var menu = new AppController();

      Application.Run();
    }

    static bool IsProcessExisted()
    {
      var name = Process.GetCurrentProcess().ProcessName;
      var processes = Process.GetProcessesByName(name);

      return processes.Length > 1;
    }
  }

}

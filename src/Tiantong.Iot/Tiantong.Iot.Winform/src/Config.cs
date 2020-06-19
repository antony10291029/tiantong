using System.Linq;
using System.Windows.Forms;

namespace Tiantong.Iot.Winforms
{
  public class Config
  {
    public string Port
    {
      get => GetPort();
      set => SetPort(value);
    }

    public Config()
    {
      using (var db = new WinformDbContext()) {
        db.Database.EnsureCreated();
      }
    }

    private string GetPort()
    {
      using (var db = new WinformDbContext()) {
        var value = db.KeyValues.FirstOrDefault(kv => kv.key == "port")?.value;

        return value ?? "5000";
      }
    }

    private void SetPort(string value)
    {
      using (var db = new WinformDbContext()) {
        var keyValue = db.KeyValues.FirstOrDefault(kv => kv.key == "port");

        if (keyValue == null) {
          keyValue = new KeyValue {
            key = "port",
          };

          db.KeyValues.Add(keyValue);
        }

        keyValue.value = value;

        db.SaveChanges();
      }
    }

  }

}

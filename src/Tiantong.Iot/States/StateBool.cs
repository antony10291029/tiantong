using System.Text.Json;

namespace Tiantong.Iot
{
  public class StateBool : State<bool>
  {
    public override void SetString(string data)
    {
      var value = JsonSerializer.Deserialize<bool>(data);

      HandleSet(value);
    }

    protected override void HandleDriverBuild()
    {
      _driver.UseBool();
    }

    protected override bool HandleGet()
    {
      return _driver.GetBool();
    }

    protected override void HandleSet(bool data)
    {
      _driver.SetBool(data);
    }
  }
}

using System.Text.Json;

namespace Tiantong.Iot
{
  public class StateBool : State<bool>
  {
    protected override bool FromString(string data)
    {
      return JsonSerializer.Deserialize<bool>(data);
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
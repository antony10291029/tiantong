namespace Tiantong.Iot
{
  public class StateBool : State<bool>
  {
    protected override bool FromString(string data)
    {
      switch (data) {
        case null:
        case "":
        case "0":
        case "false":
        case "False":
        case "FALSE":
          return false;
        default:
          return true;
      }
    }

    protected override string ToString(bool value)
    {
      return value ? "1" : "0";
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
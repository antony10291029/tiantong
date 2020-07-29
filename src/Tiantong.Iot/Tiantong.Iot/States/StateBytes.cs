using System;

namespace Tiantong.Iot
{
  public class StateBytes : State<byte[]>
  {
    public override byte[] FromString(string data)
    {
      throw new Exception("暂时不支持该类型");
    }

    protected override void HandleDriverBuild()
    {
      _driver.UseBytes(_length);
    }

    protected override byte[] HandleGet()
    {
      return _driver.GetBytes();
    }

    protected override void HandleSet(byte[] data)
    {
      _driver.SetBytes(data);
    }
  }
}

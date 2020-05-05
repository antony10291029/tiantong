using System;

namespace Tiantong.Iot
{
  public class StateBytes : State<byte[]>
  {
    protected override void HandleDriverBuild()
    {
      _driver.UseBytes(_length);
    }

    protected override int CompareTo(byte[] data, byte[] value)
    {
      throw new Exception("byte[] 类型不支持比较");
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

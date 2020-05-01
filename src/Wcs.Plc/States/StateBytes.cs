using System;

namespace Wcs.Plc
{
  public class StateBytes : State<byte[]>, IStateBytes
  {
    protected override void HandleDriverResolved()
    {
      Driver.UseBytes(Length);
    }

    protected override int CompareDataTo(byte[] data, byte[] value)
    {
      throw new Exception("byte[] 类型不支持比较");
    }

    protected override byte[] HandleGet()
    {
      return Driver.GetBytes();
    }

    protected override void HandleSet(byte[] data)
    {
      Driver.SetBytes(data);
    }
  }
}
using System;

namespace Tiantong.Iot.Protocol
{
  public interface IPlcReadRequest
  {
    byte[] Message { get; }

    void UseBool()
    {
      throw new Exception("bool type is not supported");
    }

    void UseUInt16();

    void UseInt32();

    void UseString(int length);

    void UseBytes(int length)
    {
      throw new Exception("bytes type is not supported");
    }

    void UseAddress(string address);

  }
}

using System;

namespace Tiantong.Iot.Protocol
{
  public interface IPlcReadResponse
  {
    byte[] Message { get; set; }

    void UseBool()
    {
      throw new Exception("bool type is not supported");
    }

    void UseUInt16();

    void UseInt32();

    void UseUInt32()
    {
      throw new Exception("uint32 type is not supported");
    }

    void UseString(int length);

    void UseBytes(int length)
    {
      throw new Exception("bytes type is not supported");
    }

    //

    bool GetBool()
    {
      throw new Exception("bool type is not supported");
    }

    ushort GetUInt16();

    int GetInt32();

    string GetString();

    byte[] GetBytes()
    {
      throw new Exception("bytes type is not supported");
    }

  }

}

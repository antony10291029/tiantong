using System;

namespace Wcs.Plc.Protocol
{
  public interface IPlcReadResponse
  {
    byte[] Message { get; set; }

    void UseBool();

    void UseUInt16();

    void UseInt32();

    void UseString(int length);

    void UseBytes(int length);

    //

    bool GetBool();

    ushort GetUInt16();

    int GetInt32();

    string GetString();

    byte[] GetBytes();

  }
}

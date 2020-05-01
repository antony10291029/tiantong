using System;
using System.Text;

namespace Wcs.Plc.Protocol
{
  public class S7WriteRequest: S7ReadRequest, IPlcWriteRequest
  {
    private int _length;

    private void UseDataCount(int count)
    {
      _length = count;
      Array.Resize(ref _msg, 31 + 4 + count);

      Message[31] = 0x00;
      Message[32] = 0x04;
      Message[33] = (byte)(count * 8 / 256);
      Message[34] = (byte)(count * 8 % 256);
      Message[15] = (byte)((count + 4) / 256);
      Message[16] = (byte)((count + 4) % 256);
    }

    public new void UseBool()
    {
      base.UseBool();
    }

    public new void UseUInt16()
    {
      base.UseUInt16();
      UseDataCount(2);
    }

    public new void UseInt32()
    {
      base.UseInt32();
    }

    public new void UseString(int length)
    {
      base.UseString(length);
      UseDataCount(length + 1);
    }

    public new void UseBytes(int length)
    {
      base.UseBytes(length);
    }

    public new void UseAddress(string address)
    {
      UseWriteCommand();
      SetAddress(address);
      SetMessageLength();
    }

    public void SetDataValue(byte[] data)
    {
      if (data.Length != _length) {
        Array.Resize(ref data, _length);
      }

      Array.Copy(data, 0, Message, 19 + 12 + 4, _length);
    }

    public void UseData(byte[] data)
    {
      SetDataValue(data);
    }

    public void UseData(ushort data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(int data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(bool data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(string data)
    {
      UseData(Encoding.ASCII.GetBytes((char)_length + data));
    }

  }
}

using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class S7WriteRequest: S7ReadRequest, IPlcWriteRequest
  {
    private int _length;

    protected new void UseDataCount(int count)
    {
      base.UseDataCount(count);
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
      UseWriteCommand();
      UseDataCount(2);
    }

    public new void UseInt32()
    {
      UseWriteCommand();
      UseDataCount(4);
    }

    public new void UseString(int length)
    {
      UseWriteCommand();
      UseDataCount(length + 1);
    }

    public new void UseBytes(int length)
    {
      base.UseBytes(length);
    }

    public new void UseAddress(string address)
    {
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

    public void UseData(byte[] data, bool reverse = true)
    {
      if (reverse) {
        Array.Reverse(data);
      }

      SetDataValue(data);
    }

    public void UseData(ushort data)
    {
      UseData(BitConverter.GetBytes(data), true);
    }

    public void UseData(int data)
    {
      UseData(BitConverter.GetBytes(data), true);
    }

    public void UseData(bool data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(string data)
    {
      UseData(Encoding.ASCII.GetBytes((char)_length + data), false);
    }

    public void UseData(byte[] data)
    {
      UseData(data, false);
    }
  }
}
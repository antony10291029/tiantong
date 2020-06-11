using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class MC3EBinaryWriteRequest: MC3EBinaryReadRequest, IPlcWriteRequest
  {
    private int _length;

    protected new void UseDataCount(int count)
    {
      base.UseDataCount(count);
      _length = count * 2;
      Array.Resize(ref _msg, 21 + _length);
    }

    public new void UseBool()
    {
      UseCommandWriteBit();
      UseDataCount(1);
    }

    public new void UseUInt16()
    {
      UseCommandWriteBit16();
      UseDataCount(1);
    }

    public new void UseInt32()
    {
      UseCommandWriteBit16();
      UseDataCount(2);
    }

    public new void UseString(int length)
    {
      UseCommandWriteBit16();
      UseDataCount((int) Math.Ceiling(length / 2.0));
    }

    public new void UseAddress(string address)
    {
      UseCommandWriteBit16();
      SetAddress(address);
      SetMessageLength();
    }

    public void UseData(byte[] data)
    {
      if (data.Length != _length) {
        Array.Resize(ref data, _length);
      }

      Array.Copy(data, 0, Message, 21, _length);
    }

    public void UseData(bool data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(ushort data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(int data)
    {
      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(string data)
    {
      UseData(Encoding.ASCII.GetBytes(data));
    }
  }
}
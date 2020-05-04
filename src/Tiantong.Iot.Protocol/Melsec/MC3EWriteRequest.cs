using System;
using System.Text;

namespace Tiantong.Iot.Protocol
{
  public class MC3EWriteRequest: MC3EReadRequest, IPlcWriteRequest
  {
    private int _length;

    private void UseDataCount(int count)
    {
      _length = count * 2;
      Array.Resize(ref _msg, 21 + _length);
    }

    public new void UseBool()
    {
      base.UseBool();
    }

    public new void UseUInt16()
    {
      base.UseUInt16();
      UseDataCount(1);
    }

    public new void UseInt32()
    {
      base.UseInt32();
    }

    public new void UseString(int length)
    {
      base.UseString(length);
      UseDataCount((int) Math.Ceiling(length / 2.0));
    }

    public new void UseBytes(int length)
    {
      base.UseBytes(length);
    }

    public new void UseAddress(string address)
    {
      UseCommandWriteInt16();
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
      UseData(Encoding.ASCII.GetBytes(data));
    }

  }
}

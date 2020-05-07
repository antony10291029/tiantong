using System;
using System.Text;

namespace Wcs.Plc.Protocol
{
  public class MC3EWriteRequest: MC3EReadRequest, IPlcWriteRequest
  {
    private int _length;

    private void SetDataLength(int length)
    {
      Array.Resize(ref _msg, 21 + length * 2);
    }

    public new void UseAddress(string address, int length)
    {
      _length = length;
      UseWriteCommand();
      SetAddress(address, length);
      SetDataLength(length);
      SetMessageLength();
    }

    public void SetDataValue(byte[] data)
    {
      Array.Copy(data, 0, Message, 21, _length * 2);
    }

    public byte[] UseData(byte[] data)
    {
      SetDataValue(data);

      return Message;
    }

    public byte[] UseData(int data)
    {
      var bytes = BitConverter.GetBytes((ushort) data);
      UseData(bytes);

      return Message;
    }

    public byte[] UseData(bool data)
    {
      throw new Exception("bool type is not supported");
    }

    public byte[] UseData(string data)
    {
      var expectedLength = _length * 2;

      if (data.Length > expectedLength) {
        data = data.Substring(0, expectedLength);
      } else if (data.Length < expectedLength) {
        data = data + new string((char) 0x00, expectedLength - data.Length);
      }

      var bytes = Encoding.ASCII.GetBytes(data);

      UseData(bytes);

      return Message;
    }

  }
}

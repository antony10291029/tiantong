using System;
using System.Text;

namespace Wcs.Plc.Protocol
{
  public class S7WriteRequest: S7ReadRequest, IPlcWriteRequest
  {
    private int _length;

    private void SetDataLength(int length)
    {
      Array.Resize(ref _msg, 31 + 4 + length);

      Message[31] = 0x00;
      Message[32] = 0x04;
      Message[33] = (byte)(length * 8 / 256);
      Message[34] = (byte)(length * 8 % 256);
      Message[15] = (byte)((length + 4) / 256);
      Message[16] = (byte)((length + 4) % 256);
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
      var length = Message.Length - 19 - 12 - 4;

      if (length != data.Length) {
        throw new Exception("data length is invalid");
      }

      Array.Copy(data, 0, Message, 19 + 12 + 4, data.Length);
    }

    public byte[] UseData(byte[] data)
    {
      SetDataValue(data);

      return Message;
    }

    public byte[] UseData(int data)
    {
      if (_length != 4) {
        throw new Exception("int length must be 4");
      }

      var bytes = BitConverter.GetBytes(data);
      Array.Reverse(bytes);
      UseData(bytes);

      return Message;
    }

    public byte[] UseData(bool data)
    {
      if (_length != 1) {
        throw new Exception("bool data length must be 1");
      }

      UseData(BitConverter.GetBytes(data));

      return Message;
    }

    public byte[] UseData(string data)
    {
      var expectedLength = _length - 1;

      if (data.Length > expectedLength) {
        data = data.Substring(0, expectedLength);
      } else if (data.Length < expectedLength) {
        data = data + new string((char) 0x00, expectedLength - data.Length);
      }

      data = (char)expectedLength + data;

      var bytes = Encoding.ASCII.GetBytes(data);

      UseData(bytes);

      return Message;
    }

  }
}

using System;
using System.Text;

namespace Wcs.Plc.Snap7
{
  public class S7Request
  {
    private S7RequestMessage _readMessage;

    private S7RequestMessage _writeMessage;

    public byte[] CheckHead1;

    public byte[] CheckHead2;

    public byte[] ReadMessage { get => _readMessage.Message; }

    public byte[] WriteMessage { get => _writeMessage.Message; }

    private string _key;

    private int _length;

    public S7Request()
    {
      _readMessage = new S7RequestMessage();
      _writeMessage = new S7RequestMessage();
      _readMessage.UseReadCommand();
      _writeMessage.UseWriteCommand();
    }

    public void Use200Smart()
    {
      CheckHead1 = new byte[] {
        0x03, 0x00, 0x00, 0x16, 0x11, 0xE0, 0x00, 0x00,
        0x00, 0x01, 0x00, 0xC1, 0x02, 0x10, 0x00, 0xC2,
        0x02, 0x03, 0x00, 0xC0, 0x01, 0x0A
      };
      CheckHead2 = new byte[] {
        0x03, 0x00, 0x00, 0x19, 0x02, 0xF0, 0x80, 0x32,
        0x01, 0x00, 0x00, 0xCC, 0xC1, 0x00, 0x08, 0x00,
        0x00, 0xF0, 0x00, 0x00, 0x01, 0x00, 0x01, 0x03, 0xC0
      };
    }

    public void UseAddress(string key, int length = 1)
    {
      _key = key;
      _length = length;
      _readMessage.SetAddress(key, length);
      _writeMessage.SetAddress(key, length);
      _writeMessage.UseData(length);
    }

    public void UseData(byte[] data)
    {
      _writeMessage.SetDataValue(data);
    }

    public void UseData(int data)
    {
      if (_length != 4) {
        throw new Exception("int length must be 4");
      }

      var bytes = BitConverter.GetBytes(data);
      Array.Reverse(bytes);
      UseData(bytes);
    }

    public void UseData(bool data)
    {
      if (_length != 1) {
        throw new Exception("bool data length must be 1");
      }

      UseData(BitConverter.GetBytes(data));
    }

    public void UseData(string data)
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
    }

  }
}

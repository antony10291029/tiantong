using System;
using System.Text;

namespace Wcs.Plc.Melsec
{
  public class MC3ERequest
  {
    private MC3ERequestMessage _readMessage;

    private MC3ERequestMessage _writeMessage;

    public byte[] ReadMessage { get => _readMessage.Message; }

    public byte[] WriteMessage { get => _writeMessage.Message; }

    private string _key;

    private int _length;

    public MC3ERequest()
    {
      _readMessage = new MC3ERequestMessage();
      _writeMessage = new MC3ERequestMessage();
      _readMessage.UseReadCommand();
      _writeMessage.UseWriteCommand();
    }

    public void UseAddress(string key, int length = 1)
    {
      _key = key;
      _length = length;
      _readMessage.SetAddress(key, length);
      _readMessage.SetBodyLength();
      _writeMessage.SetAddress(key, length);
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

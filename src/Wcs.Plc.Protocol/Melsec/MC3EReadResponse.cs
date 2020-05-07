using System;
using System.Text;

namespace Wcs.Plc.Protocol
{
  public class MC3EReadResponse: IPlcReadResponse
  {
    private byte[] _message; 

    public byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetResultCode();
        GetErrorCode();
        GetData();
      }
    }

    public byte[] ResultCode;

    public byte[] ErrorCode;

    public byte[] Data;

    public int Length;

    public bool IsError
    {
      get => ResultCode[0] != 0x00 || ResultCode[1] != 0x00;
    }

    private void GetResultCode()
    {
      ResultCode = Message[9..11];
    }

    private void GetErrorCode()
    {
      if (IsError) {
        ErrorCode = Message[11..13];
      }
    }

    private void GetData()
    {
      Length = BitConverter.ToInt16(Message[7..9]) - 2;
      Data = ErrorCode = Message[11..(11 + Length)];
    }

    //

    public int GetInt32()
    {
      return BitConverter.ToUInt16(Data);
    }

    public bool GetBool()
    {
      if (Data.Length != 1) {
        throw new Exception("byte array length is not 1");
      }

      return BitConverter.ToBoolean(Data);
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(Data);
    }

    public byte[] GetBytes()
    {
      return Data;
    }

  }
}

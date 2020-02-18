using System.Text;
using System;

namespace Wcs.Plc.Snap7
{
  public class S7Response
  {
    public byte[] Response;

    public bool IsDataResponse;

    public byte[] ErrorCode;

    public byte[] Data;

    public byte DataCode;

    public void SetMessage(byte[] message)
    {
      Response = message;
      GetIsDataResponse();
      GetErrorCode();
      GetDataCode();
      GetData();
    }

    public int ToInt32()
    {
      if (Data.Length != 4) {
        throw new Exception("byte array length is not 4");
      }
      var data = new byte[Data.Length];

      Array.Copy(Data, data, data.Length);
      Array.Reverse(data);

      return BitConverter.ToInt32(data);
    }

    public override string ToString()
    {
      return Encoding.ASCII.GetString(Data, 1, Data.Length - 1);
    }

    public bool ToBoolean()
    {
      if (Data.Length != 1) {
        throw new Exception("byte array length is not 1");
      }

      return BitConverter.ToBoolean(Data);
    }

    private void GetIsDataResponse()
    {
      IsDataResponse = Response[8] == 0x03;
    }

    private void GetErrorCode()
    {
      ErrorCode = new [] { Response[17], Response[18] };
    }

    private void GetDataCode()
    {
      DataCode = Response[21];
    }

    private void GetData()
    {
      var length = BitConverter.ToInt32(new byte[] { Response[16], Response[15], 0, 0 }) - 4;

      if (length <= 0) {
        return;
      }

      var data = new byte[length];
      Array.Copy(Response, 25, data, 0, length);

      Data = data;
    }
  }
}

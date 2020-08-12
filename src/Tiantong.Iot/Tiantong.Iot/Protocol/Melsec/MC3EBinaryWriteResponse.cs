using System;

namespace Tiantong.Iot.Protocol
{
  public class MC3EBinaryWriteResponse: MC3EBinaryReadResponse, IPlcWriteResponse
  {
    public override byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetResultCode();
        GetErrorCode();
      }
    }

    public void CheckError()
    {
      if (_message[7] != 0x02) {
        var byteString = BitConverter.ToString(_message);
        throw new Exception($"写入结果校对失败，编码: {byteString}");
      }
    }
  }
}
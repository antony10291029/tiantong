using System;

namespace Tiantong.Iot.Protocol
{
  public class MC1EBinaryWriteResponse: MC1EBinaryReadResponse, IPlcWriteResponse
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
  }
}

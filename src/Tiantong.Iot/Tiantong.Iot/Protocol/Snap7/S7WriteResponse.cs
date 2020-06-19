namespace Tiantong.Iot.Protocol
{
  public class S7WriteResponse: IPlcWriteResponse
  {
    public byte[] Message { get; set; }
  }

}

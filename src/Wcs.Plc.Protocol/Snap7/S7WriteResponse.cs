namespace Wcs.Plc.Protocol
{
  public class S7WriteResponse: IPlcWriteResponse
  {
    public byte[] Message { get; set; }
  }
}

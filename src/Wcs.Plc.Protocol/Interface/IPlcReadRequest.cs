namespace Wcs.Plc.Protocol
{
  public interface IPlcReadRequest
  {
    byte[] Message { get; }

    void UseAddress(string address, int length);

  }
}

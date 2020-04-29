namespace Wcs.Plc.Protocol
{
  public interface IPlcWriteRequest
  {
    byte[] Message { get; }

    void UseAddress(string key, int length);

    byte[] UseData(int data);

    byte[] UseData(bool data);

    byte[] UseData(byte[] data);

    byte[] UseData(string data);

  }
}

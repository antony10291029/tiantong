namespace Wcs.Plc.Protocol
{
  public interface IPlcReadResponse
  {
    byte[] Message { get; set; }

    int GetInt32();

    bool GetBool();

    string GetString();

  }
}

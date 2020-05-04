namespace Tiantong.Iot.Protocol
{
  public interface IPlcReadRequest
  {
    byte[] Message { get; }

    void UseBool();

    void UseUInt16();

    void UseInt32();

    void UseString(int length);

    void UseBytes(int length);

    void UseAddress(string address);

  }
}

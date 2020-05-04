namespace Tiantong.Iot.Protocol
{
  public interface IPlcWriteRequest : IPlcReadRequest
  {
    void UseData(bool data);

    void UseData(ushort data);

    void UseData(int data);

    void UseData(string data);

    void UseData(byte[] data);

  }
}

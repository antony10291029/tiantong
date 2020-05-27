using System;
using Renet.Tcp;

namespace Tiantong.Iot.Protocol
{
  public class S7TcpClient: RenetTcpClient
  {
    public byte[] CheckHead1;

    public byte[] CheckHead2;

    public S7TcpClient(string host, int port): base(host, port)
    {

    }

    public S7TcpClient Use200Smart()
    {
      CheckHead1 = new byte[] {
        0x03, 0x00, 0x00, 0x16, 0x11, 0xE0, 0x00, 0x00,
        0x00, 0x01, 0x00, 0xC1, 0x02, 0x10, 0x00, 0xC2,
        0x02, 0x03, 0x00, 0xC0, 0x01, 0x0A
      };
      CheckHead2 = new byte[] {
        0x03, 0x00, 0x00, 0x19, 0x02, 0xF0, 0x80, 0x32,
        0x01, 0x00, 0x00, 0xCC, 0xC1, 0x00, 0x08, 0x00,
        0x00, 0xF0, 0x00, 0x00, 0x01, 0x00, 0x01, 0x03, 0xC0
      };

      return this;
    }

    public override void Connected()
    {
      try {
        Send(CheckHead1);
        Send(CheckHead2);
      } catch (Exception e) {
        throw new Exception($"握手数据校对错误: {e.Message}");
      }
    }
  }
}

using NUnit.Framework;

namespace Tiantong.Iot.Protocol.Test
{
  [TestFixture]
  public class MC3EBinaryWriteResponseTest
  {
    private byte[] GetMessage()
    {
      return new byte[] {
        0xD0, 0x00, 0x00, 0xFF, 0xFF, 0x03, 0x00, 0x02, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
      };
    }

    [Test]
    public void TestResponseSuccess()
    {
      var message = GetMessage();
      var response = new MC3EBinaryWriteResponse();

      response.Message = message;
    }

    [TestCase(0x00, 0x01)]
    [TestCase(0x01, 0x00)]
    [TestCase(0x01, 0x01)]
    public void TestResponseError(byte resultCode, byte errorCode)
    {
      var message = GetMessage();
      var response = new MC3EBinaryWriteResponse();

      message[9] = resultCode;
      message[10] = errorCode;

      try {
        response.Message = message;
        Assert.Fail("expect throw write response result code and error code");
      } catch {}
    }
  }

}

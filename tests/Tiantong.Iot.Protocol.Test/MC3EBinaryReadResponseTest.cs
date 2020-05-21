using NUnit.Framework;

namespace Tiantong.Iot.Protocol.Test
{
  [TestFixture]
  public class MC3EBinaryReadResponseTest
  {
    private byte[] GetMessage()
    {
      return new byte[] {
        0xD0, 0x00, 0x00, 0xFF, 0xFF, 0x03, 0x00, 0x04, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
      };
    }

    [TestCase(0x01, 0x00)]
    [TestCase(0x00, 0x01)]
    [TestCase(0x01, 0x01)]
    public void TestError(byte resultCode, byte errorCode)
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      response.UseUInt16();

      message[9] = resultCode;
      message[10] = errorCode;

      try {
        response.Message = message;
        Assert.Fail("expect throw response result code and error code");
      } catch {}
    }

    [TestCase(0x0)]
    [TestCase(0x3)]
    [TestCase(0x5)]
    [TestCase(0x0A)]
    public void TestLengthError(byte length)
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      message[7] = length;
      response.UseUInt16();

      try {
        response.Message = message;
        response.GetUInt16();
        Assert.Fail("expect data length error");
      } catch {}
    }

    [Test]
    public void TestUint16Length()
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      message[7] = 0x04;
      response.UseUInt16();
      response.Message = message;
      response.GetUInt16();
    }

    [Test]
    public void TestInt16Length()
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      message[7] = 0x04;
      response.UseInt16();
      response.Message = message;
      response.GetUInt16();
    }

    [Test]
    public void TestInt32Length()
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      message[7] = 0x06;
      response.UseInt32();
      response.Message = message;
      response.GetInt32();
    }

    [Test]
    public void TestUInt32Length()
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      message[7] = 0x06;
      response.UseUInt32();
      response.Message = message;
      response.GetUInt32();
    }

    [TestCase(1, 2)]
    [TestCase(2, 2)]
    [TestCase(9, 10)]
    [TestCase(10, 10)]
    public void TestString(int length, int expectedLength)
    {
      var message = GetMessage();
      var response = new MC3EBinaryReadResponse();

      message[7] = (byte)(expectedLength + 2);

      response.UseString(length);
      response.Message = message;
    }
  }

}

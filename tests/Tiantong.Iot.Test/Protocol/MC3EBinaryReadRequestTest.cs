using System;
using NUnit.Framework;

namespace Tiantong.Iot.Protocol.Test
{
  [TestFixture]
  public class MC3EBinaryReadRequestTest
  {
    [Test]
    public void TestMessageLength()
    {
      var request = new MC3EBinaryReadRequest();

      request.UseAddress("D100");

      Assert.AreEqual(12, BitConverter.ToUInt16(new byte[] {
        request.Message[7],
        request.Message[8],
      }));
    }

    private void AssertCommandReadBit16(byte[] message)
    {
      Assert.AreEqual(message[11], 0x01);
      Assert.AreEqual(message[12], 0x04);
      Assert.AreEqual(message[13], 0x00);
      Assert.AreEqual(message[14], 0x00);
    }

    private void AssertDataCount(byte[] message, int count)
    {

      Assert.AreEqual(count, BitConverter.ToUInt16(message[19..21]));
    }

    // Type tests

    [Test]
    public void TestInt16()
    {
      var request = new MC3EBinaryReadRequest();

      request.UseUInt16();

      AssertDataCount(request.Message, 1);
      AssertCommandReadBit16(request.Message);
    }

    [Test]
    public void TestUseUint16()
    {
      var request = new MC3EBinaryReadRequest();

      request.UseUInt16();

      AssertDataCount(request.Message, 1);
      AssertCommandReadBit16(request.Message);
    }

    [Test]
    public void TestInt32()
    {
      var request = new MC3EBinaryReadRequest();

      request.UseInt32();

      AssertDataCount(request.Message, 2);
      AssertCommandReadBit16(request.Message);
    }

    [Test]
    public void TestUInt32()
    {
      var request = new MC3EBinaryReadRequest();

      request.UseUInt32();

      AssertDataCount(request.Message, 2);
      AssertCommandReadBit16(request.Message);
    }

    [TestCase(1, 1)]
    [TestCase(2, 1)]
    [TestCase(5, 3)]
    [TestCase(6, 3)]
    [TestCase(100, 50)]
    public void TestString(int length, int dataCount)
    {
      var request = new MC3EBinaryReadRequest();

      request.UseString(length);

      AssertDataCount(request.Message, dataCount);
      AssertCommandReadBit16(request.Message);
    }

    [Test]
    public void TestStringEmpty()
    {
      var request = new MC3EBinaryReadRequest();

      try {
        request.UseString(0);
        Assert.Fail("expect an error when string length is zero");
      } catch {
        Assert.True(true);
      }
    }

    [TestCase(256)]
    [TestCase(99999)]
    public void TestStringOverLength(int length)
    {
      var request = new MC3EBinaryReadRequest();

      try {
        request.UseString(length);
        Assert.Fail("expect string length over error");
      } catch {
        Assert.True(true);
      }
    }

    //

    private void AssertAddressType(byte[] message, string type)
    {
      Assert.AreEqual(message[18], type switch {
        "M" => 0x90,
        "V" => 0x94,
        "B" => 0xA0,
        "D" => 0xA8,
        "W" => 0xB4,
        _ => throw new Exception("Data type is invalid"),
      });
    }

    private void AssertAddressOffset(byte[] message, int offset)
    {
      Assert.AreEqual(offset, BitConverter.ToInt32(new byte[] {
        message[15], message[16], message[17], 0x00
      }));
    }

    [TestCase("M0", "M", 0)]
    [TestCase("V1", "V", 1)]
    [TestCase("B1", "B", 1)]
    [TestCase("D1", "D", 1)]
    [TestCase("W1", "W", 1)]
    [TestCase("D*100", "D", 100)]
    [TestCase("W100", "W", 0x100)]
    [TestCase("D999999", "D", 999999)]
    [TestCase("B999999", "B", 0x999999)]
    public void TestAddress(string address, string type, int offset)
    {
      var request = new MC3EBinaryReadRequest();

      request.UseAddress(address);

      AssertAddressType(request.Message, type);
      AssertAddressOffset(request.Message, offset);
    }

    [TestCase("A1")]
    [TestCase("C2")]
    [TestCase("3D")]
    public void TestAddressTypeError(string address)
    {
      var request = new MC3EBinaryReadRequest();

      try {
        request.UseAddress(address);
        Assert.Fail("expect address type error");
      } catch {}
    }

  }

}

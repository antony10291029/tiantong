using System;
using System.Globalization;

namespace Tiantong.Iot.Protocol
{
  public class MC1EBinaryReadRequest: BinaryMessage, IPlcReadRequest
  {
    protected byte[] _msg = new byte[] {
      0x00,  // 0.  参数 - 指令
      0xFF,  // 1.  固定 - 可编程控制器编号
      0x28,  // 2.  参数 - 等待时间 (结束代码) - 1
      0x00,  // 3.  参数 - 等待时间 (结束代码) - 2

      0x00,  // 4.  参数 - 软元件地址 - 1
      0x00,  // 5.  参数 - 软元件地址 - 2
      0x00,  // 6.  参数 - 软元件地址 - 3
      0x00,  // 7.  参数 - 软元件地址 - 3

      0x00,  // 8.  参数 - 软元件代码 - 1
      0x00,  // 9.  参数 - 软元件代码 - 2

      0x00,  // 10. 参数 - 软元件点数 - 1
      0x00,  // 11. 参数 - 软元件点数 - 2
    };

    public override byte[] Message { get => _msg; }

    //

    private (string, int) TransAddress(string addr)
    {
      if (addr.Length < 2) {
        throw KnownException.Error();
      }

      var type = "D";
      var offset = 0;

      if (!int.TryParse(addr[1].ToString(), out _)) {
        type = addr.Substring(0, 2);
        addr = addr.Substring(2);
      } else if (!int.TryParse(addr[0].ToString(), out _)) {
        type = addr.Substring(0, 1);
        addr = addr.Substring(1);
      } else {
        throw KnownException.Error();
      }

      type = type.TrimEnd('*');
      if (type == "B" || type == "W") {
        offset = int.Parse(addr, NumberStyles.HexNumber);
      } else {
        offset = int.Parse(addr);
      }

      return (type, offset);
    }

    //

    protected void UseCommandReadBit()
    {
      Message[0] = 0x00;
    }

    protected void UseCommandWriteBit()
    {
      Message[0] = 0x02;
    }

    protected void UseCommandRead16Bit()
    {
      Message[0] = 0x01;
    }

    protected void UseCommandWriteBit16()
    {
      Message[0] = 0x03;
    }

    private void SetDataType(string type)
    {
       switch (type) {
        case "M":
          Message[8] = 0x20;
          Message[9] = 0x4D;
          break;
        case "B":
          Message[8] = 0x20;
          Message[9] = 0x42;
          break;
        case "D":
          Message[8] = 0x20;
          Message[9] = 0x44;
          break;
        case "W":
          Message[8] = 0x20;
          Message[9] = 0x57;
          break;
        default: throw KnownException.Error("不支持该类型的软元件读写");
      }
    }

    private void SetDateOffset(int offset)
    {
      var bytes = BitConverter.GetBytes(offset);

      Message[4] = bytes[0];
      Message[5] = bytes[1];
      Message[6] = bytes[2];
      Message[7] = bytes[3];
    }

    protected void SetAddress(string address)
    {
      try {
        var (type, offset) = TransAddress(address);

        SetDataType(type);
        SetDateOffset(offset);
      } catch (Exception e) {
        throw KnownException.Error($"{e.Message}: {address}");
      }
    }

    //

    public void UseBool()
    {
      UseCommandReadBit();
      UseDataCount(1);
    }

    public void UseUInt16()
    {
      UseCommandRead16Bit();
      UseDataCount(1);
    }

    public void UseInt32()
    {
      UseCommandRead16Bit();
      UseDataCount(2);
    }

    public void UseUInt32()
    {
      UseCommandRead16Bit();
      UseDataCount(2);
    }

    public void UseString(int length)
    {
      if (length == 0) {
        throw KnownException.Error("字符串长度至少为1");
      }
      if (length >= 255) {
        throw KnownException.Error("字符串长度不可超过255");
      }

      UseCommandRead16Bit();
      UseDataCount((int) Math.Ceiling(length / 2.0));
    }

    protected void UseDataCount(int count)
    {
      var bytes = BitConverter.GetBytes(count);

      Message[10] = bytes[0];
      Message[11] = bytes[1];
    }

    public void UseAddress(string address)
    {
      SetAddress(address);
    }
  }
}

using Renet;
using System;
using System.Globalization;

namespace Tiantong.Iot.Protocol
{
  public class MC3EBinaryReadRequest: BinaryMessage, IPlcReadRequest
  {
    protected byte[] _msg = new byte[] {
      0x50,    // 0.  固定 - 帧头 - 1
      0x00,    // 1.  固定 - 帧头 - 2

      0x00,    // 2.  固定 - 网络编号
      0xFF,    // 3.  固定 - 可编程控制器编号
      0xFF,    // 4.  固定 - 请求目标模块IO编号 - 1
      0x03,    // 5.  固定 - 请求目标模块IO编号 - 2
      0x00,    // 6.  固定 - 请求目标模块站号

      0x0C,    // 7.  参数 - 数据长度 - 1
      0x00,    // 8.  参数 - 数据长度 - 2

      0x00,    // 9.  参数 - 等待时间 (结束代码) - 1
      0x00,    // 10. 参数 - 等待时间 (结束代码) - 2

      0x00,    // 11. 参数 - 指令 - 1
      0x00,    // 12. 参数 - 指令 - 2
      0x00,    // 13. 参数 - 子指令 - 1
      0x00,    // 14. 参数 - 子指令 - 2

      0x00,    // 15. 参数 - 软元件地址 - 1
      0x00,    // 16. 参数 - 软元件地址 - 1
      0x00,    // 17. 参数 - 软元件地址 - 1
      0x00,    // 18. 参数 - 软元件代码

      0x00,    // 19. 参数 - 软元件点数 - 1
      0x00,    // 20. 参数 - 软元件点数 - 2

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
      Message[11] = 0x01;
      Message[12] = 0x04;
      Message[13] = 0x01;
      Message[14] = 0x00;
    }

    protected void UseCommandWriteBit()
    {
      Message[11] = 0x00;
      Message[12] = 0x04;
      Message[13] = 0x01;
      Message[14] = 0x00;
    }

    protected void UseCommandRead16Bit()
    {
      Message[11] = 0x01;
      Message[12] = 0x04;
      Message[13] = 0x00;
      Message[14] = 0x00;
    }

    protected void UseCommandWriteBit16()
    {
      Message[11] = 0x01;
      Message[12] = 0x14;
      Message[13] = 0x00;
      Message[14] = 0x00;
    }

    private void SetDataType(string type)
    {
      Message[18] = type switch {
        "M" => 0x90,
        "V" => 0x94,
        "B" => 0xA0,
        "D" => 0xA8,
        "W" => 0xB4,
        _ => throw KnownException.Error("不支持该类型的软元件读写")
      };

      if (Message[13] == 0x01) {
        switch (type) {
          case "D":
          case "W":
            throw KnownException.Error("该类型的数据必须使用位软元件地址");
        }
      }
    }

    private void SetDateOffset(int offset)
    {
      Message[15] = (byte)(offset % 256);               // 9.  偏移位置 - 1
      Message[16] = (byte)(offset / 256 % 256);         // 10. 偏移位置 - 2
      Message[17] = (byte)(offset / 256 / 256 % 256);   // 11. 偏移位置 - 3
    }

    protected void SetMessageLength()
    {
      Message[7] = (byte)((Message.Length - 9) % 256);
      Message[8] = (byte)((Message.Length - 9) / 256);
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
      Message[19] = (byte)(count % 256);
      Message[20] = (byte)(count / 256);
    }

    public void UseAddress(string address)
    {
      SetAddress(address);
    }
  }
}
using Renet;
using System;

namespace Tiantong.Iot.Protocol
{
  public class S7ReadRequest: IPlcReadRequest
  {
    protected byte[] _msg = new byte[] {
      0x03,    // 0.  报文头
      0x00,    // 1.  报文头
      0x00,    // 2.  参数 - 长度(1)
      0x00,    // 3.  参数 - 长度(2)
      0x02,    // 4.  固定
      0xF0,    // 5.  固定
      0x80,    // 6.  固定
      0x32,    // 7.  协议标识
      0x01,    // 8.  消息类型，01 为请求，02 为响应，03 为带数据的响应
      0x00,    // 9.  请求 ID - 1
      0x00,    // 10. 请求 ID - 2
      0x00,    // 11. 数据协议长度 - 1
      0X01,    // 12. 数据协议长度 - 2
      0x00,    // 13. 参数长度 - 1
      0x0E,    // 14. 参数长度 - 2 读和写参数长度固定为 14
      0x00,    // 15. 数据长度 - 1
      0x00,    // 16. 数据长度 - 2
      0x04,    // 17. 读写指令：04 度，05 写
      0x01,    // 18. 读取数据块个数
      0x12,    // 19. 指定有效值类型 Specify a valid value type
      0x0A,    // 20. 接下来本次地址访问长度 -> The next time the address access length
      0x10,    // 21. 语法标记，ANY -> Syntax tag, any
      0x02,    // 22. 按字 (word) 为单位
      0x00,    // 23. 数据点数量 - 1
      0x00,    // 24. 数据点数量 - 2
      0x00,    // 25. DB 块长度 - 1
      0x00,    // 26. DB 块长度 - 2
      0x00,    // 27. 数据类型
      0x00,    // 28. 偏移位置 - 1
      0x00,    // 29. 偏移位置 - 2
      0x00,    // 30. 偏移位置 - 3
    };

    public byte[] Message { get => _msg; }

    public void UseReadCommand()
    {
      Message[17] = 0x04;
    }

    public void UseWriteCommand()
    {
      Message[17] = 0x05;
    }

    //

    private byte TransDataType(string prefix)
    {
      switch (prefix) {
        case "I" : return 0x81;
        case "Q" : return 0x82;
        case "M" : return 0x83;
        case "C" : return 0x1C;
        case "T" : return 0x1D;
        case "V" : return 0x87;
        case "D" : return 0x84;
        case "DB" : return 0x84;
        default : throw KnownException.Error();
      }
    }

    private int TransAddressOffset(string offset)
    {
      var result = 0;
      var arr = offset.Split(".");

      result += int.Parse(arr[0]) * 8;

      if (arr.Length == 2) {
        result += int.Parse(arr[1]);
      }

      return result;
    }

    private int TransDbBlock(string block)
    {
      try {
        return int.Parse(block);
      } catch {
        throw KnownException.Error();
      }
    }

    private (string, int, int) TransAddress(string addr)
    {
      if (addr.Length < 2) {
        throw KnownException.Error();
      }

      var type = "D";
      var block = "0";
      var offset = "";

      if (!int.TryParse(addr[1].ToString(), out _)) {
        type = addr.Substring(0, 2);
        addr = addr.Substring(2);
      } else if (!int.TryParse(addr[0].ToString(), out _)) {
        type = addr.Substring(0, 1);
        addr = addr.Substring(1);
      } else {
        throw KnownException.Error();
      }

      offset = addr;

      if (type == "D" || type == "DB") {
        var arr = addr.Split(".");
        block = arr[0];
        offset = string.Join(".", arr.Length == 3
          ? new [] { arr[1], arr[2] }
          : new [] { arr[1] }
        );
      }
      if (type == "V") {
        type = "D";
        block = "1";
      }

      return (type, TransAddressOffset(offset), TransDbBlock(block));
    }

    //

    public void SetMessageLength()
    {
      Message[2] = (byte)(Message.Length / 256);
      Message[3] = (byte)(Message.Length % 256);
    }

    private void SetParameterDbBlock(int block)
    {
      Message[25] = (byte)(block / 256);              // 6.  DB 块长度 - 1
      Message[26] = (byte)(block % 256);              // 7.  DB 块长度 - 2
    }

    private void SetParameterDataType(string type)
    {
      Message[27] = TransDataType(type);
    }

    private void SetParameterOffset(int offset)
    {
      Message[28] = (byte)(offset / 256 / 256 % 256); // 9.  偏移位置 - 1
      Message[29] = (byte)(offset / 256 % 256);       // 10. 偏移位置 - 2
      Message[30] = (byte)(offset % 256);             // 11. 偏移位置 - 3
    }

    protected void SetAddress(string address)
    {
      try {
        var (db, offset, block) = TransAddress(address);

        SetParameterDbBlock(block);
        SetParameterDataType(db);
        SetParameterOffset(offset);
      } catch {
        throw KnownException.Error($"PLC地址格式错误: {address}");
      }
    }

    protected void UseDataCount(int count)
    {
      Message[23] = (byte)(count / 256);             // 4.  数据点数量 - 1
      Message[24] = (byte)(count % 256);             // 5.  数据点数量 - 2
    }

    public void UseBool()
    {
      throw KnownException.Error("暂时不支持 Bool 类型");
    }

    public void UseUInt16()
    {
      UseReadCommand();
      UseDataCount(2);
    }

    public void UseInt32()
    {
      UseReadCommand();
      UseDataCount(4);
    }

    public void UseString(int length)
    {
      UseReadCommand();
      UseDataCount(length + 1);
    }

    public void UseBytes(int length)
    {
      throw KnownException.Error("暂时不支持 Bytes 类型");
    }

    public void UseAddress(string address)
    {
      SetAddress(address);
      SetMessageLength();
    }
  }
}
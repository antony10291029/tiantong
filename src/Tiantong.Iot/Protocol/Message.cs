using System;

namespace Tiantong.Iot.Protocol
{
  public class BinaryMessage
  {
    public virtual byte[] Message { get; set; }

    public void Dump()
    {
      var byteString = BitConverter.ToString(Message);

      Console.WriteLine(byteString);
    }

  }

}

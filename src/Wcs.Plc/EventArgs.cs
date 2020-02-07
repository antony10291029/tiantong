namespace Wcs.Plc
{
  public class EventArgs
  {
    public string Key { get; set; }

    public string Payload { get; set; }

    public int HandlerCount { get; set; }
  }
}

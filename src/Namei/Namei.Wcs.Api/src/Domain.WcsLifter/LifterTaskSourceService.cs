using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public record LifterTaskSource
  {
    public string Destination { get; set; }

    public string Data { get; set; }

    public string From { get; set; }
  }

  public interface ILifterTaskSourceService
  {
    LifterTaskSource FindFromBarcode(string barcode);

    void RequestToPick(string lifterId, string floor, string barcode, string data, string from);
  }

  public class LifterTaskSourceService: ILifterTaskSourceService
  {
    private readonly IWmsService _wms;

    public LifterTaskSourceService(IWmsService wms)
    {
      _wms = wms;
    }

    public LifterTaskSource FindFromBarcode(string barcode)
    {
      var result = _wms.GetPalletInfo(barcode).GetAwaiter().GetResult();

      return new() {
        Destination = result.Destination,
        Data = result.TaskId,
        From = LifterTaskFrom.Wms
      };
    }

    public void RequestToPick(string lifterId, string floor, string barcode, string data, string from)
    {
      _wms.RequestPicking(lifterId, floor, barcode, data);
    }
  }
}

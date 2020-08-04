namespace Namei.Wcs.Api
{
  public class PalletInfo
  {
    public string Destination { get; set; } = "2";

    public string TaskId { get; set; } = "0001";
  }

  public class WmsService
  {
    public static string Destination = "1";

    public PalletInfo GetPalletInfo(string barcode)
    {
      return new PalletInfo() {
        Destination = Destination,
      };
    }

    public void RequestPicking(int lifterId, string floor, string code, string taskId)
    {

    }
  }
}

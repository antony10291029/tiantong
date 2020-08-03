namespace Namei.Wcs.Api
{
  public class PalletInfo
  {
    public string Destination { get; set; } = "2";

    public string TaskId { get; set; } = "0001";
  }

  public class WmsService
  {
    public PalletInfo GetPalletInfo(string barcode)
    {
      return new PalletInfo();
    }

    public void RequestPicking(int lifterId, string floor, string code, string taskId)
    {

    }
  }
}

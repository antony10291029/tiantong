using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class LifterCommandController: BaseController
  {
    private ILifterServiceFactory _lifters;

    private ICapPublisher _cap;

    private IWmsService _wms;

    public LifterCommandController(
      ICapPublisher cap,
      ILifterServiceFactory lifters,
      IWmsService wms
    ) {
      _cap = cap;
      _lifters = lifters;
      _wms = wms;
    }

    public class StandardLifterConveyorChangedParams
    {
      public string lifter_id { get; set; }

      public string floor { get; set; }

      public string value { get; set; }
    }

  }
}

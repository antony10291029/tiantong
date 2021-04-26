using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Namei.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private WmsContext _wms;

    public AppController(Config config, WmsContext wms)
    {
      _config = config;
      _wms = wms;
    }

    [Route("/")]
    public object Home()
    {
      return new {
        name = "Namei.Wcs.Common"
      };
    }
  }
}

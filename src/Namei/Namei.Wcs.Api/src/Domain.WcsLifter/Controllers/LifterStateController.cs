using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterStateController: BaseController
  {
    private readonly ILifterServiceFactory _lifters;

    public LifterStateController(ILifterServiceFactory lifters)
    {
      _lifters = lifters;
    }

    [HttpPost("/lifters/states")]
    public object GetLifterStates()
      => _lifters.All().ToDictionary(kv => kv.Key, kv => kv.Value.GetStates());
  }
}

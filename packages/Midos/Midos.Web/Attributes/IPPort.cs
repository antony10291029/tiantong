using System.ComponentModel.DataAnnotations;

namespace Midos.Web
{
  public class IPPortAttribute: RangeAttribute
  {
    public IPPortAttribute(): base(1, 65535)
    {
      ErrorMessage = "端口范围必须在 1 至 65535";
    }
  }
}

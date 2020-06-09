using System;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace Renet.Web.Attributes
{
  public class IPPortAttribute: RangeAttribute
  {
    public IPPortAttribute(): base(1, 65535)
    {
      ErrorMessage = "端口范围必须在 1 至 65535";
    }
  }
}
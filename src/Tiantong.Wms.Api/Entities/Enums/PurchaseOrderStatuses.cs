using System.Collections.Generic;

namespace Tiantong.Wms.Api
{
  public static class PurchaseOrderStatuses
  {
    public const string Created = "created";

    public const string Finished = "finished";

    public const string Filed = "filed";

    public static string[] All = new string[] { "created", "finished", "filed" };
  }
}

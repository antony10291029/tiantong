using System.Collections.Generic;
namespace Tiantong.Wms.Api
{
  public static class UserTypes
  {
    public const string Root = "root";

    public const string Owner = "owner";

    public const string Keeper = "keeper";

    public static string[] AllowedTypes = new string[] { "owner", "keeper" };
  }
}

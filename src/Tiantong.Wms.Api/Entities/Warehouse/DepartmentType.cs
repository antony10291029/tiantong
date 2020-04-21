namespace Tiantong.Wms.Api
{
  public static class DepartmentType
  {
    public const string Owner = "owner";

    public const string Admin = "admin";

    public const string User = "user";

    public const string Custom = "custom";

    public static string[] SystemTypes
    {
      get => new string[] { Owner, Admin, User };
    }
  }
}

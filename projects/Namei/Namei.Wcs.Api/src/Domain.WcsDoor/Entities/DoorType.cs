using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Namei.Wcs.Api
{
  public class DoorType
  {
    public const string Automatic = "自动门";

    public const string Crash = "防撞门";

    public const string Table = "接驳装置门";

    public static string GetPlcName(string doorId)
      => doorId switch {
        "101" => "自动门 - 1F - 1",
        "102" => "自动门 - 1F - 1",
        "103" => "自动门 - 1F - 1",
        "104" => "自动门 - 1F - 2",
        "105" => "自动门 - 1F - 2",
        "106" => "自动门 - 1F - 3",
        "107" => "自动门 - 1F - 3",
        "201" => "自动门 - 2F",
        "202" => "自动门 - 2F",
        _ => throw KnownException.Error($"自动门不存在: {doorId}")
      };

    public static string GetFloor(string doorId)
      => doorId.Substring(0, 1);

    public static string GetLifterId(string doorId)
      => (int.Parse(doorId.Substring(1, 2)) - 50).ToString();

    public static string GetDoorIdFromLifter(string floor, string lifterId)
      => $"{floor}5{lifterId}";

    public static ReadOnlyDictionary<string, string> Map
      = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
        { "101", Automatic },
        { "102", Automatic },
        { "103", Automatic },
        { "104", Automatic },
        { "105", Automatic },
        { "106", Automatic },
        { "107", Automatic },
        { "201", Automatic },
        { "202", Automatic },
        { "151", Crash },
        { "251", Crash },
        { "351", Crash },
        { "451", Crash },
        { "152", Crash },
        { "252", Crash },
        { "352", Crash },
        { "452", Crash },
        { "153", Crash },
        { "253", Crash },
        { "353", Crash },
        { "453", Crash },
      });
  }
}

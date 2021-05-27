using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Namei.Wcs.Api.Test;

namespace Namei.Wcs.Aggregates.Test
{
  [TestClass]
  public class RcsMapServiceTest
  {
    [TestMethod]
    public void Test_To_DataNames()
    {
      var locations = new TcsMapData[] {
        new() { MapDataCode = "0001@0001", DataName = "000001" },
        new() { MapDataCode = "0001@0002", DataName = "000002" },
        new() { MapDataCode = "0001@0003", DataName = "000003" },
      };
      var context = Utils.GetRcsContext();
      var service = new RcsMapService(context);

      context.AddRange(locations);
      context.SaveChanges();

      var codes = service.ToDataName(
        locations.Select(code => code.MapDataCode).ToArray()
      );

      for (var i = 0; i < locations.Length; i++) {
        Assert.AreEqual(locations[i].DataName, codes[i]);
      }
    }

    [TestMethod]
    public void Test_Get_Free_Location_Code_Null()
    {
      var context = Utils.GetRcsContext();
      var service = new RcsMapService(context);
      var location = service.GetFreeLocationCode("100");

      Assert.IsNull(location);
    }

    [TestMethod]
    public void Test_Get_Free_Location_Code()
    {
      var locations = new TcsMapData[] {
        new() { MapDataCode = "002@0001", DataName = "000021", PodCode = null,  AreaCode = "003", WcsAreaSeq = 1 },
        new() { MapDataCode = "002@0002", DataName = "000022", PodCode = null,  AreaCode = "002", WcsAreaSeq = 2 },
        new() { MapDataCode = "002@0003", DataName = "000023", PodCode = "100000",  AreaCode = "002", WcsAreaSeq = 3 },
        new() { MapDataCode = "002@0004", DataName = "000024", PodCode = null,  AreaCode = "002", WcsAreaSeq = 4 },
        new() { MapDataCode = "002@0005", DataName = "000025", PodCode = null,  AreaCode = "002", WcsAreaSeq = 5 },
        new() { MapDataCode = "002@0006", DataName = "000026", PodCode = null,  AreaCode = "002", WcsAreaSeq = 6 },
      };
      var tasks = new TcsMainTask[] {
        new() {
          MainTaskNum = "0001",
          TaskStatus = TcsMainTaskStatus.Started,
          ViaCodes = JsonSerializer.Serialize(new string[] { "002@0001", "002@0002" })
        }
      };

      var context = Utils.GetRcsContext();
      var service = new RcsMapService(context);

      context.AddRange(locations);
      context.AddRange(tasks);
      context.SaveChanges();

      var location = service.GetFreeLocationCode("002");

      Assert.AreEqual("000024", location);
    }
  }
}

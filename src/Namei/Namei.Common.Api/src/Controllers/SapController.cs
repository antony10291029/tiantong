using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Namei.Common.Api
{
  public class SapController
  {
    private readonly SapContext _sap;

    private readonly WmsContext _wms;

    private readonly MesContext _mes;

    public SapController(SapContext sap, WmsContext wms, MesContext mes)
    {
      _sap = sap;
      _wms = wms;
      _mes = mes;
    }

    public record DTO
    {
      public string SecondClassify { get; set; }

      public string ThirdClassify { get; set; }

      public string ItemCode { get; set; }

      public string WarehouseCode { get; set; }

      public string BatchCode { get; set; }

      public string ItemName { get; set; }

      public string Unit { get; set; }

      public decimal SapQuantity { get; set; }

      public decimal OtherQuantity { get; set; }

      public decimal DiffQuantity { get; set; }

      public string OtherSystem { get; set; }
    }

    static readonly string[] LogisticWarehouses = new string[] {
      "101", "103", "104", "108", "201", "301", "302", "307", "412", "903"
    };

    [HttpGet("/SapMesWms/Inventories")]
    [Produces("application/xml")]
    public object GetInventorys([FromQuery] string warehouse)
    {
      var sapQuery = _sap.Set<SapItemWarehouseInventory>().AsQueryable();
      var wmsQuery = _wms.Set<WmsItemWarehouseInventory>().AsQueryable();
      var mesQuery = _mes.Set<MesItemWarehouseInventory>().AsQueryable();

      if (warehouse == "wms") {
        sapQuery = sapQuery.Where(item => LogisticWarehouses.Contains(item.WarehouseCode));
        mesQuery = mesQuery.Where(item => false);
        wmsQuery = wmsQuery.Where(item => true);
      } else if (!string.IsNullOrWhiteSpace(warehouse)) {
        sapQuery = sapQuery.Where(item => item.WarehouseCode == warehouse);
        mesQuery = mesQuery.Where(item => item.WarehouseCode == warehouse);
        wmsQuery = wmsQuery.Where(item => false);
      }

      var sapData = sapQuery.ToArray().ToDictionary(item => $"{item.ItemCode},{item.WarehouseCode},{item.BatchCode}", item => item);
      var wmsData = wmsQuery.ToArray().ToDictionary(item => $"{item.ItemCode},{item.WarehouseCode},{item.BatchCode}", item => item);
      var mesData = mesQuery.ToArray().ToDictionary(item => $"{item.ItemCode},{item.WarehouseCode},{item.BatchCode}", item => item);

      var result = sapData.Select(item => {
        var otherSystem = "无";
        var otherQuantity = default(decimal);

        if (wmsData.ContainsKey(item.Key)) {
          otherSystem = "wms";
          otherQuantity = (decimal) wmsData[item.Key].Quantity;
        } else if (mesData.ContainsKey(item.Key)) {
          otherSystem = "mes";
          otherQuantity = (decimal) mesData[item.Key].Quantity;
        }

        return new DTO() {
          SecondClassify = item.Value.U_I002,
          ThirdClassify = item.Value.U_I003,
          WarehouseCode = item.Value.WarehouseCode,
          BatchCode = item.Value.BatchCode,
          ItemCode = item.Value.ItemCode,
          ItemName = item.Value.ItemName,
          Unit = item.Value.Unit,
          SapQuantity = item.Value.Quantity,
          OtherQuantity = otherQuantity,
          DiffQuantity = item.Value.Quantity - otherQuantity,
          OtherSystem = otherSystem
        };
      });

      return result
        .Where(dto => dto.OtherSystem != "无")
        .Where(dto => Math.Abs(dto.DiffQuantity) > 0)
        .OrderBy(dto => dto.WarehouseCode)
        .ThenBy(dto => dto.ItemCode)
        .ThenBy(dto => dto.BatchCode)
        .ToArray();
    }
  }
}

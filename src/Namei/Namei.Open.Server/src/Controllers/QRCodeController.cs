using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Google.Protobuf.WellKnownTypes;
using System.Reflection;

namespace Namei.Open.Server
{
  public class QRCodeController
  {
    private readonly SapContext _sap;

    private readonly MesContext _mes;

    private readonly WmsContext _wms;

    public QRCodeController(SapContext sap, MesContext mes, WmsContext wms)
    {
      _mes = mes;
      _sap = sap;
      _wms = wms;
    }

    [HttpPost("/test")]
    public object Test([FromBody] QueryParams param)
    {
      var codeInfo = new QRCodeInfo();

      var record = _mes.Set<MesRetrospectCode>().FirstOrDefault(item => item.ItemBarCode == param.Code);
      var mesOrder = _mes.Set<MesWoOrder>().FirstOrDefault(item => item.WoOrderNo == record.WoOrderNo);
      var boxCode = record?.BoxBarCode;
      var batchId = record?.MaterielLot;
      var itemCode = mesOrder.FormulaNo;

      var item = _sap.Set<SapItem>().FirstOrDefault(item => item.ItemCode == itemCode);
      var itemName = item?.ItemName;

      var boxBind = _wms.Set<WmsBoxCodeBind>().FirstOrDefault(item => item.BoxCode == boxCode);
      var moveDocCode = boxBind?.OrderCode;
      var moveDoc = _wms.Set<WmsMoveDoc>().FirstOrDefault(item => item.Code == moveDocCode);
      var pickTicketId = moveDoc?.RelatedBillId;
      var pickTicket = _wms.Set<WmsPickTicket>().FirstOrDefault(item => item.Id == pickTicketId);
      var shipToName = pickTicket?.ShipToName;
      var shipDate = boxBind?.BindTime;

      return new {
        boxCode,
        batchId,
        itemCode,
        itemName,
        shipToName,
        shipDate,
      };
    }

    public record QueryParams
    {
      public string Type { get; set; }

      public string Code { get; set; }
    }

    public record SearchRecordRow
    {
      public string Key { get; set; }

      public string Value { get; set; }
    }

    public record QRCodeInfo
    {
      public string BoxCode { get; set; }

      public string BatchId { get; set; }

      public string ItemCode { get; set; }

      public string ItemName { get; set; }

      public string ShipToName { get; set; }

      public string ShipDate { get; set; }
    }

    public record SearchResult
    {
      public string Title { get; set; }

      public List<SearchRecordRow> Records { get; set; } = new();
    }

    [HttpPost("/qrcode/search")]
    public SearchResult Search([FromBody] QueryParams param)
    {
      var now = DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss");
      var record = _mes.Set<MesRetrospectCode>().FirstOrDefault(item => item.ItemBarCode == param.Code);

      if (record == null) {
        return new() {
          Title = "无效编码",
          Records = new() {
            new() { Key = "查询时间", Value = now }
          }
        };
      }

      var mesOrder = _mes.Set<MesWoOrder>().FirstOrDefault(item => item.WoOrderNo == record.WoOrderNo);
      var boxCode = record?.BoxBarCode;
      var batchId = record?.MaterielLot;
      var itemCode = mesOrder.FormulaNo;

      var item = _sap.Set<SapItem>().FirstOrDefault(item => item.ItemCode == itemCode);
      var itemName = item?.ItemName;

      var boxBind = _wms.Set<WmsBoxCodeBind>().FirstOrDefault(item => item.BoxCode == boxCode);
      var moveDocCode = boxBind?.OrderCode;
      var moveDoc = _wms.Set<WmsMoveDoc>().FirstOrDefault(item => item.Code == moveDocCode);
      var pickTicketId = moveDoc?.RelatedBillId;
      var pickTicket = _wms.Set<WmsPickTicket>().FirstOrDefault(item => item.Id == pickTicketId);
      var shipToName = pickTicket?.ShipToName;
      var shipDate = boxBind?.BindTime.ToString("yyyy-MM-dd HH:MM:ss");

      return new() {
        Title = "一品一码追溯",
        Records = new() {
          new() { Key = "产品编码", Value = itemCode },
          new() { Key = "产品名称", Value = itemName },
          new() { Key = "产品批次", Value = batchId },
          new() { Key = "经销商", Value = shipToName },
          new() { Key = "发货时间", Value = shipDate },
          new() { Key = "查询时间", Value = now },
        }
      };
    }
  }
}

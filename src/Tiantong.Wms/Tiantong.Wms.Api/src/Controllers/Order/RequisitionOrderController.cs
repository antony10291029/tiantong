using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class RequisitionOrder : BaseController
  {
    private RequisitionOrderRepository _orders;

    public RequisitionOrder(RequisitionOrderRepository orders)
    {
      _orders = orders;
    }

    public object Create([FromBody] Order order)
    {
      _orders.Add(order);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("领料单已创建", order.id);
    }

    public class FindParams
    {
      public int order_id { get; set; }

      public int warehouse_id { get; set; }
    }

    public object Delete([FromBody] FindParams param)
    {
      _orders.Remove(param.warehouse_id, param.order_id);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("领料单已删除");
    }

    public object Update([FromBody] Order order)
    {
      _orders.Update(order);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("领料单已更新");
    }

    public class FinishParams
    {
      public int warehouse_id { get; set; }

      public int location_id { get; set; }

      public int order_id { get; set; }
    }

    public object Finish([FromBody] FinishParams param)
    {
      _orders.Finish(param.warehouse_id, param.order_id, param.location_id);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("领料单确认出库");
    }

    public object File([FromBody] FindParams param)
    {
      _orders.File(param.warehouse_id, param.order_id);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("领料单已归档");
    }

    public object Restore([FromBody] FindParams param)
    {
      _orders.Restore(param.warehouse_id, param.order_id);
      _orders.UnitOfWork.SaveChanges();

      return SuccessOperation("领料单已恢复");
    }

    public object Find([FromBody] FindParams param)
    {
      return _orders.Find(param.warehouse_id, param.order_id, OrderType.Requisition);
    }

    public class SearchParams : BaseSearchParams
    {
      public int warehouse_id { get; set; }

      public string status { get; set; }
    }

    public IPagination<Order> Search([FromBody] SearchParams param)
    {
      return _orders.Search(
        param.warehouse_id,
        OrderType.Requisition,
        param.status,
        param.search,
        param.page,
        param.page_size
      );
    }

  }
}

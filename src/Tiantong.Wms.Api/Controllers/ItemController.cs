using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemController: BaseController
  {
    private IAuth _auth;

    private GoodRepository _goods;

    private ItemRepository _items;

    private WarehouseRepository _warehouses;

    public ItemController(
      IAuth auth,
      GoodRepository goods,
      ItemRepository items,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _goods = goods;
      _items = items;
      _warehouses = warehouses;
    }

    public class CreateParams
    {
      public int good_id { get; set; }

      public string number { get; set; }

      public string name { get; set; } = "";

      public string unit { get; set; } = "";
    }

    public object Create([FromBody] CreateParams param)
    {
      _auth.EnsureOwner();
      var good = _goods.EnsureGetByOwner(param.good_id, _auth.User.id);

      _items.UnitOfWork.BeginTransaction();

      var item = _items.Add(new Item {
        good_id = param.good_id,
        warehouse_id = good.warehouse_id,
        number = param.number,
        name = param.name,
        unit = param.unit
      });
      _items.UnitOfWork.SaveChanges();
      // good.item_ids.Add(item.id);
      _items.UnitOfWork.SaveChanges();
      _items.UnitOfWork.Commit();

      return SuccessOperation("规格已添加", item.id);
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      var item = _items.EnsureGet(param.id);
      _warehouses.EnsureOwner(item.warehouse_id, _auth.User.id);
      // if (item.stock_ids.Count > 0) {
      //   return FailureOperation("规格已被使用，无法删除");
      // }
      var good = _goods.EnsureGet(item.good_id);

      _items.UnitOfWork.BeginTransaction();
      _items.Remove(item);
      // good.item_ids.Remove(item.id);
      _items.UnitOfWork.SaveChanges();
      _items.UnitOfWork.Commit();

      return SuccessOperation("规格已删除");
    }
  }
}

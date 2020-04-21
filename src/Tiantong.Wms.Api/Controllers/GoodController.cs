using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class GoodController : BaseController
  {
    private Auth _auth;

    private GoodRepository _goods;

    private StockRepository _stocks;

    private LocationRepository _locations;

    private WarehouseRepository _warehouses;

    private GoodCategoryRepository _goodCategories;

    public GoodController(
      Auth auth,
      GoodRepository goods,
      ItemRepository items,
      StockRepository stocks,
      LocationRepository locations,
      WarehouseRepository warehouses,
      GoodCategoryRepository itemCategories
    ) {
      _auth = auth;
      _goods = goods;
      _stocks = stocks;
      _locations = locations;
      _warehouses = warehouses;
      _goodCategories = itemCategories;
    }

    public object Create([FromBody] Good good)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(good.warehouse_id, _auth.User.id);

      _goods.Add(good);
      _goods.UnitOfWork.SaveChanges();

      return SuccessOperation("货品已创建", good.id);
    }

    public class AddGoodItemParams
    {
      public int warehouse_id { get; set; }

      public string good_name { get; set; }

      public string item_name { get; set; }

      public string item_unit { get; set; }
    }

    public object AddGoodItem([FromBody] AddGoodItemParams param)
    {
      _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);

      var good = _goods.Table.Include(g => g.items)
        .FirstOrDefault(g =>
          g.name == param.good_name &&
          g.warehouse_id == param.warehouse_id
        );

      var item = new Item();
      item.name = param.item_name;
      item.unit = param.item_unit;
      item.warehouse_id = param.warehouse_id;

      if (good == null) {
        good = new Good();
        good.warehouse_id = param.warehouse_id;
        good.name = param.good_name;
        good.items = new List<Item> { item };
        _goods.EnsureUnique(good);
        _goods.DbContext.Add(good);
      } else {
        item.good_id = good.id;
        _goods.Items.EnsureUnique(item);
        _goods.DbContext.Add(item);
      }

      _goods.UnitOfWork.SaveChanges();

      return SuccessOperation("货品规格已添加");
    }

    public class DeleteParams
    {
      [Nonzero]
      public int id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      var good = _goods.EnsureGet(param.id);
      _warehouses.EnsureUser(good.warehouse_id, _auth.User.id);

      _goods.Remove(good);
      _goods.UnitOfWork.SaveChanges();

      return SuccessOperation("货品已删除");
    }

    public object Update([FromBody] Good good)
    {
      _auth.EnsureUser();
      _warehouses.EnsureUser(good.warehouse_id, _auth.User.id);
      _goods.Update(good);
      _goods.UnitOfWork.SaveChanges();

      return SuccessOperation("货品信息已保存");
    }

    public class SearchParams : BaseSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public IPagination<Good> Search([FromBody] SearchParams param)
    {
      _auth.EnsureUser();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureUser(warehouseId, _auth.User.id);

      var goods = _goods.Table
        .Include(good => good.items)
          .ThenInclude(item => item.stocks)
        .Where(good =>
          good.warehouse_id == param.warehouse_id && 
          (param.search == null ? true : (
            good.name.Contains(param.search) ||
            good.number.Contains(param.search) ||
            good.items.Any(item => item.name.Contains(param.search)) ||
            good.items.Any(item => item.number.Contains(param.search))
          ))
        )
        .Paginate<Good>(param.page, param.page_size);

      foreach (var good in goods.Data.Values) {
        good.items = good.items.OrderBy(item => item.index).ToList();
      }

      return goods;
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    public Good Find([FromBody] FindParams param)
    {
      _auth.EnsureUser();

      var good = _goods.EnsureGet(param.id);
      _warehouses.EnsureUser(good.warehouse_id, _auth.User.id);
      good.items = good.items.OrderBy(item => item.index).ToList();

      return good;
    }

    public class ItemDeletableParams
    {
      public int id { get; set; }
    }

    public object IsItemRemovable([FromBody] ItemDeletableParams param)
    {
      var item = _goods.Items.EnsureGet(param.id);
      var good = _goods.EnsureGet(item.good_id);
      _warehouses.EnsureUser(good.warehouse_id, _auth.User.id);
      if (_goods.Items.isRemovable(item)) {
        return new { deletable = true };
      } else {
        return FailureOperation("该规格已被使用，不可删除");
      }
    }
  }
}

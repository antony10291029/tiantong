using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemController : BaseController
  {
    private IAuth _auth;

    private ItemRepository _items;

    private WarehouseRepository _warehouses;

    private ItemCategoryRepository _itemCategories;

    public ItemController(
      IAuth auth,
      ItemRepository items,
      WarehouseRepository warehouses,
      ItemCategoryRepository itemCategories
    ) {
      _auth = auth;
      _items = items;
      _warehouses = warehouses;
      _itemCategories = itemCategories;
    }

    public class ItemCreateParams
    {
      [Required]
      public int? warehouse_id { get; set; }

      [Required]
      public string number { get; set; }

      public int[] category_ids { get; set; } = new int[] {};

      [Required]
      public string name { get; set; }

      [Required]
      public string specification { get; set; }

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] ItemCreateParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);
      _items.EnsureNumberUnique(warehouseId, param.number);
      _itemCategories.EnsureIds(warehouseId, param.category_ids);

      var item = new Item {
        warehouse_id = warehouseId,
        number = param.number,
        category_ids = param.category_ids,
        name = param.name,
        specification = param.specification,
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _items.Add(item);
      _items.UnitOfWork.SaveChanges();

      return new {
        message = "Success to create item",
        id = item.id
      };
    }

    public class ItemDeleteParams
    {
      [Required]
      public int? id { get; set; }
    }

    public object Delete([FromBody] ItemDeleteParams param)
    {
      _auth.EnsureOwner();
      var item = _items.EnsureGetByOwner((int) param.id, _auth.User.id);
      _items.Remove(item.id);
      _items.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete item");
    }

    public class ItemUpdateParams
    {
      [Required]
      public int? id { get; set; }

      public string number { get; set; }

      public int[] categroy_ids { get; set; }

      public string name { get; set; }

      public string specification { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] ItemUpdateParams param)
    {
      _auth.EnsureOwner();
      var item = _items.EnsureGetByOwner((int) param.id, _auth.User.id);
      if (param.number != null) {
        _items.EnsureNumberUnique(item.warehouse_id, param.number);
        item.number = param.number;
      }
      if (param.categroy_ids != null) {
        _itemCategories.EnsureIds(item.warehouse_id, param.categroy_ids);
        item.category_ids = param.categroy_ids;
      }
      if (param.name != null) item.name = param.name;
      if (param.specification != null) item.specification = param.specification;
      if (param.comment != null) item.comment = param.comment;
      if (param.is_enabled != null) {
        item.is_enabled = (bool) param.is_enabled;
      }
      _items.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update item");
    }

    public class ItemSearchParams
    {
      [Required]
      public int? warehouse_id { get; set; }
    }

    public Item[] Search([FromBody] ItemSearchParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);

      return _items.Search(warehouseId);
    }
  }
}

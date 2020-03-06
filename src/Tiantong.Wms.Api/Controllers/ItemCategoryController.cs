using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ItemCategoryController : BaseController
  {
    private IAuth _auth;

    private WarehouseRepository _warehouses;

    private ItemCategoryRepository _itemCategories;

    public ItemCategoryController(
      IAuth auth,
      WarehouseRepository warehouses,
      ItemCategoryRepository itemCategories
    ) {
      _auth = auth;
      _warehouses = warehouses;
      _itemCategories = itemCategories;
    }

    public class ItemCategoryCreateParams
    {
      [Required]
      public int? warehouse_id { get; set; }

      [Required]
      public string name { get; set; }

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] ItemCategoryCreateParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);
      _itemCategories.EnsureNameUnique(warehouseId, param.name);

      var category = new ItemCategory {
        warehouse_id = warehouseId,
        name = param.name,
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _itemCategories.Add(category);
      _itemCategories.UnitOfWork.SaveChanges();

      return new {
        message = "Success to create item category",
        id = category.id
      };
    }

    public class ItemCategoryDeleteParams
    {
      [Required]
      public int? id { get; set; }
    }

    public object Delete([FromBody] ItemCategoryDeleteParams param)
    {
      _auth.EnsureOwner();
      var itemCategory =  _itemCategories.EnsureGetByOwner((int) param.id, _auth.User.id);
      _itemCategories.Remove(itemCategory.id);
      _itemCategories.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete item category");
    }

    public class ProjectUpdateParams
    {
      [Required]
      public int? id { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] ProjectUpdateParams param)
    {
      _auth.EnsureOwner();
      var category = _itemCategories.EnsureGetByOwner((int) param.id, _auth.User.id);

      if (param.comment != null) category.comment = param.comment;
      if (param.is_enabled != null) {
        category.is_enabled = (bool) param.is_enabled;
      }
      if (param.name != null) {
        _itemCategories.EnsureNameUnique(category.warehouse_id, param.name);
        category.name = param.name;
      }
      _itemCategories.UnitOfWork.SaveChanges();

      return JsonMessage("Success to update item category");
    }

    public class ItemCategorySearchParams
    {
      [Required]
      public int? warehouse_id { get; set; }
    }

    public ItemCategory[] Search([FromBody] ItemCategorySearchParams param)
    {
      _auth.EnsureOwner();
      var warehouseId = (int) param.warehouse_id;
      _warehouses.EnsureOwner(warehouseId, _auth.User.id);

      return _itemCategories.Search(warehouseId);
    }
  }
}

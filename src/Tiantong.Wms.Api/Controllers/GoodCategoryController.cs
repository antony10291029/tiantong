using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class GoodCategoryController : BaseController
  {
    private IAuth _auth;

    private WarehouseRepository _warehouses;

    private GoodRepository _goods;

    private GoodCategoryRepository _itemCategories;

    public GoodCategoryController(
      IAuth auth,
      GoodRepository goods,
      WarehouseRepository warehouses,
      GoodCategoryRepository itemCategories
    ) {
      _auth = auth;
      _goods = goods;
      _warehouses = warehouses;
      _itemCategories = itemCategories;
    }

    public class GoodCategoryCreateParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      [Required]
      public string name { get; set; }

      public string number { get; set; }

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;
    }

    public object Create([FromBody] GoodCategoryCreateParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _itemCategories.EnsureNameUnique(param.warehouse_id, param.name);

      if (param.number != null) {
        _itemCategories.EnsureNumberUnique(param.warehouse_id, param.number);
      }

      var category = new GoodCategory {
        warehouse_id = param.warehouse_id,
        name = param.name,
        number = param.number, 
        comment = param.comment,
        is_enabled = param.is_enabled,
      };
      _itemCategories.Add(category);
      _itemCategories.UnitOfWork.SaveChanges();

      return SuccessOperation("货类已创建", category.id);
    }

    public class GoodCategoryDeleteParams
    {
      [Nonzero]
      public int category_id { get; set; }
    }

    public object Delete([FromBody] GoodCategoryDeleteParams param)
    {
      _auth.EnsureOwner();
      var category =  _itemCategories.EnsureGetByOwner(param.category_id, _auth.User.id);

      if (_goods.HasCategory(category.warehouse_id, category.id)) {
        return FailureOperation("货类已使用，无法被删除");
      }

      _itemCategories.Remove(category.id);
      _itemCategories.UnitOfWork.SaveChanges();

      return SuccessOperation("货类已删除");
    }

    public class UpdateParams
    {
      [Nonzero]
      public int id { get; set; }

      public string name { get; set; }

      public string number { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }
    }

    public object Update([FromBody] UpdateParams param)
    {
      _auth.EnsureOwner();
      var category = _itemCategories.EnsureGetByOwner(param.id, _auth.User.id);

      if (param.comment != null) category.comment = param.comment;
      if (param.is_enabled != null) {
        category.is_enabled = (bool) param.is_enabled;
      }
      if (param.number != null) {
        _itemCategories.EnsureNumberUnique(category.warehouse_id, param.number);
        category.number = param.number;
      }
      if (param.name != null) {
        _itemCategories.EnsureNameUnique(category.warehouse_id, param.name);
        category.name = param.name;
      }
      _itemCategories.UnitOfWork.SaveChanges();

      return SuccessOperation("货类信息已保存");
    }

    public class SearchParams: BaseSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public IPagination<GoodCategory> Search([FromBody] SearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      return _itemCategories.Table
        .Where(category =>
          category.warehouse_id == param.warehouse_id &&
          (param.search == null ? true : category.name.Contains(param.search))
        )
        .OrderBy(category => category.id)
        .Paginate(param.page, param.page_size);
    }

    public class FindParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      [Nonzero]
      public int category_id { get; set; }
    }

    public GoodCategory Find([FromBody] FindParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      return _itemCategories.EnsureGet(param.category_id, param.warehouse_id);
    }
  }
}

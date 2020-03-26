// using System.Linq;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Renet.Web;

// namespace Tiantong.Wms.Api
// {
//   public class ItemController : BaseController
//   {
//     private IAuth _auth;

//     private GoodRepository _items;

//     private StockRepository _stocks;

//     private LocationRepository _locations;

//     private OrderItemRepository _orderItems;

//     private WarehouseRepository _warehouses;

//     private ItemCategoryRepository _itemCategories;

//     public ItemController(
//       IAuth auth,
//       GoodRepository items,
//       StockRepository stocks,
//       LocationRepository locations,
//       OrderGoodRepository orderItems,
//       WarehouseRepository warehouses,
//       ItemCategoryRepository itemCategories
//     ) {
//       _auth = auth;
//       _items = items;
//       _stocks = stocks;
//       _locations = locations;
//       _orderItems = orderItems;
//       _warehouses = warehouses;
//       _itemCategories = itemCategories;
//     }

//     public class CreateParams
//     {
//       [Nonzero]
//       public int warehouse_id { get; set; }

//       public string number { get; set; }

//       public int[] category_ids { get; set; } = new int[] {};

//       [Required]
//       public string name { get; set; }

//       [Required]
//       public string specification { get; set; }

//       public string comment { get; set; } = "";
//     }

//     public object Create([FromBody] CreateParams param)
//     {
//       _auth.EnsureOwner();
//       _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
//       _itemCategories.EnsureIds(param.warehouse_id, param.category_ids);

//       var location = _locations.Table
//         .Where(ltn => ltn.warehouse_id == param.warehouse_id)
//         .First();
//       var category = _itemCategories.Table
//         .Where(ctg => ctg.warehouse_id == param.warehouse_id)
//         .First();

//       param.category_ids = new int[] { category.id };

//       if (param.number != null) {
//         _items.EnsureNumberUnique(param.warehouse_id, param.number);
//       }

//       var item = new Item {
//         warehouse_id = param.warehouse_id,
//         number = param.number,
//         category_ids = param.category_ids,
//         name = param.name,
//         specification = param.specification,
//         comment = param.comment,
//       };
//       _items.Add(item);

//       _items.UnitOfWork.BeginTransaction();

//       _items.UnitOfWork.SaveChanges();
//       _stocks.Add(new Stock {
//         warehouse_id = param.warehouse_id,
//         item_id = item.id,
//         location_id = location.id
//       });
//       _items.UnitOfWork.SaveChanges();
//       _items.UnitOfWork.Commit();

//       return SuccessOperation("货品已创建", item.id);
//     }

//     public class DeleteParams
//     {
//       [Nonzero]
//       public int id { get; set; }
//     }

//     public object Delete([FromBody] DeleteParams param)
//     {
//       _auth.EnsureOwner();
//       var item = _items.EnsureGetByOwner(param.id, _auth.User.id);
//       if (_orderItems.HasItem(param.id)) {
//         return FailureOperation("货品已使用，无法被删除");
//       }
//       _items.Remove(item.id);
//       _items.UnitOfWork.SaveChanges();

//       return SuccessOperation("货品已删除");
//     }

//     public class UpdateParams
//     {
//       [Nonzero]
//       public int id { get; set; }

//       public string number { get; set; }

//       public int[] categroy_ids { get; set; }

//       public string name { get; set; }

//       public string specification { get; set; }

//       public string comment { get; set; }

//       public bool? is_enabled { get; set; }
//     }

//     public object Update([FromBody] UpdateParams param)
//     {
//       _auth.EnsureOwner();
//       var item = _items.EnsureGetByOwner(param.id, _auth.User.id);
//       if (param.number != null) {
//         _items.EnsureNumberUnique(item.warehouse_id, param.number);
//         item.number = param.number;
//       }
//       if (param.categroy_ids != null) {
//         _itemCategories.EnsureIds(item.warehouse_id, param.categroy_ids);
//         item.category_ids = param.categroy_ids;
//       }
//       if (param.name != null) item.name = param.name;
//       if (param.specification != null) item.specification = param.specification;
//       if (param.comment != null) item.comment = param.comment;
//       if (param.is_enabled != null) {
//         item.is_enabled = (bool) param.is_enabled;
//       }
//       _items.UnitOfWork.SaveChanges();

//       return SuccessOperation("货品信息已保存");
//     }

//     public class SearchParams : BaseSearchParams
//     {
//       [Nonzero]
//       public int warehouse_id { get; set; }
//     }

//     public IPagination<Item> Search([FromBody] SearchParams param)
//     {
//       _auth.EnsureOwner();
//       var warehouseId = (int) param.warehouse_id;
//       _warehouses.EnsureOwner(warehouseId, _auth.User.id);

//       return _items.Table.Include(item => item.stocks).Where(item =>
//         item.warehouse_id == param.warehouse_id && (
//           param.search == null ? true :
//           item.name.Contains(param.search) ||
//           item.number.Contains(param.search)
//         ))
//         .OrderByDescending(item => item.is_enabled)
//         .ThenBy(item => item.number)
//         .ThenBy(item => item.id)
//         .Paginate(param.page, param.page_size);
//     }

//     public class FindParams
//     {
//       public int id { get; set; }
//     }

//     public Item Find([FromBody] FindParams param)
//     {
//       _auth.EnsureOwner();

//       return _items.EnsureGetByOwner(param.id, _auth.User.id);
//     }
//   }
// }

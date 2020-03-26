// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Mvc;
// using Renet.Web;

// namespace Tiantong.Wms.Api
// {
//   public class OrderCategoryController : BaseController
//   {
//     private IAuth _auth;

//     private WarehouseRepository _warehouses;

//     private OrderCategoryRepository _orderCategories;

//     public OrderCategoryController(
//       IAuth auth,
//       WarehouseRepository warehouses,
//       OrderCategoryRepository orderCategories
//     ) {
//       _auth = auth;
//       _warehouses = warehouses;
//       _orderCategories = orderCategories;
//     }

//     public class OrderCategoryCreateParams
//     {
//       [Required]
//       public int? warehouse_id { get; set; }

//       [Required]
//       public string type { get; set; }

//       [Required]
//       public string name { get; set; }

//       public string comment { get; set; } = "";

//       public bool is_enabled { get; set; } = true;
//     }

//     public object Create([FromBody] OrderCategoryCreateParams param)
//     {
//       _auth.EnsureOwner();
//       var warehouseId = (int) param.warehouse_id;
//       _warehouses.EnsureOwner(warehouseId, _auth.User.id);
//       _orderCategories.EnsureNameUnique(warehouseId, param.type, param.name);

//       var category = new OrderCategory {
//         warehouse_id = warehouseId,
//         type = param.type,
//         name = param.name,
//         comment = param.comment,
//         is_enabled = param.is_enabled,
//       };
//       _orderCategories.Add(category);
//       _orderCategories.UnitOfWork.SaveChanges();

//       return new {
//         message = "Success to create order category",
//         id = category.id
//       };
//     }

//     public class OrderCategoryDeleteParams
//     {
//       [Required]
//       public int? id { get; set; }
//     }

//     public object Delete([FromBody] OrderCategoryDeleteParams param)
//     {
//       _auth.EnsureOwner();
//       var OrderCategory =  _orderCategories.EnsureGetByOwner((int) param.id, _auth.User.id);
//       _orderCategories.Remove(OrderCategory.id);
//       _orderCategories.UnitOfWork.SaveChanges();

//       return JsonMessage("Success to delete order category");
//     }

//     public class ProjectUpdateParams
//     {
//       [Required]
//       public int? id { get; set; }

//       public string name { get; set; }

//       public string comment { get; set; }

//       public bool? is_enabled { get; set; }
//     }

//     public object Update([FromBody] ProjectUpdateParams param)
//     {
//       _auth.EnsureOwner();
//       var category = _orderCategories.EnsureGetByOwner((int) param.id, _auth.User.id);

//       if (param.comment != null) category.comment = param.comment;
//       if (param.is_enabled != null) {
//         category.is_enabled = (bool) param.is_enabled;
//       }
//       if (param.name != null) {
//         _orderCategories.EnsureNameUnique(category.warehouse_id, category.type, param.name);
//         category.name = param.name;
//       }
//       _orderCategories.UnitOfWork.SaveChanges();

//       return JsonMessage("Success to update order category");
//     }

//     public class OrderCategorySearchParams
//     {
//       [Required]
//       public int? warehouse_id { get; set; }
//     }

//     public OrderCategory[] Search([FromBody] OrderCategorySearchParams param)
//     {
//       _auth.EnsureOwner();
//       var warehouseId = (int) param.warehouse_id;
//       _warehouses.EnsureOwner(warehouseId, _auth.User.id);

//       return _orderCategories.Search(warehouseId);
//     }
//   }
// }

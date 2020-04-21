// using System;
// using System.Linq;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Mvc;
// using Renet.Web;

// namespace Tiantong.Wms.Api
// {
//   public class OrderInboundCompleteController : BaseController
//   {
//     private Auth _auth;

//     private ItemRepository _items;

//     private OrderRepository _orders;

//     private StockRepository _stocks;

//     private SupplierRepository _suppliers;

//     private LocationRepository _locations;

//     private OrderItemRepository _orderItems;

//     private WarehouseRepository _warehouses;

//     private StockRecordRepository _stockRecords;

//     private OrderCategoryRepository _orderCategories;

//     public OrderInboundCompleteController(
//       Auth auth,
//       ItemRepository items,
//       OrderRepository orders,
//       StockRepository stocks,
//       LocationRepository locations,
//       SupplierRepository suppliers,
//       OrderItemRepository orderItems,
//       WarehouseRepository warehouses,
//       StockRecordRepository stockRecords,
//       OrderCategoryRepository orderCategories
//     ) {
//       _auth = auth;
//       _items = items;
//       _orders = orders;
//       _stocks = stocks;
//       _suppliers = suppliers;
//       _locations = locations;
//       _orderItems = orderItems;
//       _warehouses = warehouses;
//       _stockRecords = stockRecords;
//       _orderCategories = orderCategories;
//     }

//     public class Params
//     {
//       [Nonzero]
//       public int warehouse_id { get; set; }

//       [Required]
//       public string number { get; set; }

//       [Nonzero]
//       public int category_id { get; set; }

//       [Nonzero]
//       public int supplier_id { get; set; }

//       public string status { get; set; } = "";

//       public string comment { get; set; } = "";

//       [Required]
//       public ItemParams[] items { get; set; }

//     }

//     public class ItemParams
//     {
//       public int key { get; set; }

//       [Nonzero]
//       public int item_id { get; set; }

//       [Nonzero]
//       public int location_id { get; set; }

//       public double price { get; set; } = 0;

//       public int quantity { get; set; } = 1;

//       public int arrived_quantity { get; set; } = 1;
//     }

//     public object Handle([FromBody] Params param)
//     {
//       // _auth.EnsureUser();

//       // var itemKeys = param.items.Select(item => item.key).ToArray();
//       // var itemIds = param.items.Select(item => item.item_id).ToArray();
//       // var locationIds = param.items.Select(item => item.location_id).ToArray();

//       // _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);
//       // _orders.EnsureNumberUnique(param.warehouse_id, param.number);
//       // _orderCategories.EnsureId(param.warehouse_id, param.category_id);
//       // _suppliers.EnsureId(param.warehouse_id, param.supplier_id);
//       // _orderItems.EnsureKeys(itemKeys);
//       // _items.EnsureIds(param.warehouse_id, itemIds);
//       // _locations.EnsureIds(param.warehouse_id, locationIds);

//       // var order = new Order {
//       //   warehouse_id = param.warehouse_id,
//       //   number = param.number,
//       //   type = "inbound",
//       //   operator_id = _auth.User.id,
//       //   status = param.status,
//       //   comment = param.comment,
//       //   category_id = param.category_id,
//       //   due_time = DateTime.Now,
//       //   finished_at = DateTime.Now,
//       // };
//       // var orderItems = param.items.Select(item => new OrderItem {
//       //   item_id = item.item_id,
//       //   price = item.price,
//       //   quantity = item.quantity,
//       //   arrived_quantity = item.arrived_quantity,
//       // }).ToList();
//       // var stockRecords = param.items.Select(item => new StockRecord {
//       //   warehouse_id = param.warehouse_id,
//       //   item_id = item.item_id,
//       //   location_id = item.location_id,
//       //   quantity = item.arrived_quantity,
//       // }).ToList();

//       // var stocks = param.items.Select(item => {
//       //   var stock = _stocks.GetOrAdd(
//       //     param.warehouse_id, item.item_id, item.location_id
//       //   );
//       //   stock.quantity += item.arrived_quantity;
//       //   return stock;
//       // }).ToList();

//       // _orders.Add(order);

//       // _orders.UnitOfWork.BeginTransaction();
//       // _orders.UnitOfWork.SaveChanges();

//       // orderItems.ForEach(item => item.order_id = order.id);
//       // stockRecords.ForEach(item => item.order_id = order.id);
//       // _orderItems.AddRange(orderItems);

//       // _stockRecords.AddRange(stockRecords);
//       // _orders.UnitOfWork.SaveChanges();
//       // _orders.UnitOfWork.Commit();

//       return JsonMessage("Success to complete order");
//     }

//     public class SearchParams
//     {
//       [Nonzero]
//       public int warehouse_id { get; set; }
//     }

//     public Order[] Search([FromBody] SearchParams param)
//     {
//       _auth.EnsureUser();
//       _warehouses.EnsureUser(param.warehouse_id, _auth.User.id);

//       return _orders.Table.Where(order => order.type == "inbound").ToArray();
//     }

//   }
// }

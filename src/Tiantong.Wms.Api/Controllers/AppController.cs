using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Renet.Web;
using Tiantong.Wms.DB;
using DBCore;

namespace Tiantong.Wms.Api
{
  public class AppController : BaseController
  {
    private IAuth _auth;

    private DbContext _db;

    private MigratorProvider _migratorProvider;

    private IRandom _random;

    private IConfiguration _config;

    private UserRepository _users;

    private WarehouseRepository _warehouses;

    private AreaRepository _areas;

    private LocationRepository _locations;

    private ProjectRepository _projects;

    private SupplierRepository _suppliers;

    private ItemCategoryRepository _itemCategories;

    private OrderCategoryRepository _orderCategories;

    private ItemRepository _items;

    private StockRepository _stocks;

    private StockRecordRepository _stockRecords;

    public AppController(
      IAuth auth,
      DbContext db,
      MigratorProvider migratorProvider,
      IRandom random,
      IConfiguration config,
      UserRepository users,
      WarehouseRepository warehouses,
      AreaRepository areas,
      LocationRepository locations,
      ProjectRepository projects,
      SupplierRepository suppliers,
      ItemCategoryRepository itemCategories,
      OrderCategoryRepository orderCategories,
      ItemRepository items,
      StockRepository stocks,
      StockRecordRepository stockRecords
    ) {
      _db = db;
      _migratorProvider = migratorProvider;
      _auth = auth;
      _config = config;
      _random = random;
      _users = users;
      _warehouses = warehouses;
      _locations = locations;
      _projects = projects;
      _suppliers = suppliers;
      _areas = areas;
      _itemCategories = itemCategories;
      _orderCategories = orderCategories;
      _items = items;
      _stocks = stocks;
      _stockRecords = stockRecords;
    }

    public object Home()
    {
      return JsonMessage(_config["app_name"]);
    }

    // 初始化一个 root 用户
    // 用户 email 和 password 由配置文件提供
    // 若 root 用户已存在，则不再重复创建
    public object Initialize()
    {
      if (IsInitialized()) {
        return JsonMessage("系统初始化失败，不可重复初始化系统");
      }

      InitializeRootUser();

      return JsonMessage("系统初始化成功");
    }

    public object Migrate()
    {
      _migratorProvider.Migrator.Migrate();

      return SuccessOperation("数据库已同步");
    }

    public object Rollback()
    {
      _migratorProvider.Migrator.Rollback();

      return SuccessOperation("数据库已回档");
    }

    public object refresh()
    {
      _migratorProvider.Migrator.Refresh();

      return SuccessOperation("数据库已刷新");
    }

    private bool IsInitialized()
    {
      return _users.HasRoot();
    }

    private void InitializeRootUser()
    {
      var user = new User {
        type = UserTypes.Root,
        password = _config.GetValue("root_password", "123456"),
        email = _config.GetValue("root_email", "root@system.com"),
      };

      _users.Add(user);
      _users.UnitOfWork.SaveChanges();
    }

    public object Reseed()
    {
      _migratorProvider.Migrator.Refresh();

      return InsertTestData();
    }

    public object InsertTestData()
    {
      if (IsInitialized()) {
        return FailureOperation("系统已完成初始化");
      }

      InsertRootUser();
      InsertOwnerUsers();
      InsertWarehouses();
      InsertAreas();
      InsertLocations();
      InsertProjects();
      InsertSuppliers();
      InsertItemCategories();
      InsertOrderCategories();
      InsertItems();
      InsertStocks();
      InsertStockRecords();

      return SuccessOperation("测试数据建立完毕");
    }

    private void InsertRootUser()
    {
      if (!IsInitialized()) {
        InitializeRootUser();
      }
    }

    private void InsertOwnerUsers()
    {
      for (var i = 0; i < _random.Int(20, 100); i++) {
        var user = new User {
          type = UserTypes.Owner,
          password = "123456",
          email = "owner" + (i == 0 ? "" : i.ToString()) + "@wms.com"
        };

        _users.Add(user);
      }

      _users.UnitOfWork.SaveChanges();
    }

    private void InsertWarehouses()
    {
      _users.Owners.OrderBy(user => user.id).Take(3).ToList().ForEach(owner => {
        int L = _random.Int(3, 5);
        for (var i = 1; i < L; i++) {
          _warehouses.Add(new Warehouse() {
            owner_user_id = owner.id,
            number = $"WH000{i}",
            name = $"test warehouse {i}",
            address = $"test warehouse address {i}",
            comment = $"test warehouse comment {i}",
          });
        }
      });

      _warehouses.UnitOfWork.SaveChanges();
    }

    private void InsertAreas()
    {
      _warehouses.Table.OrderBy(item => item.id).ToList().ForEach(warehouse => {
        int L = _random.Int(5, 10);
        for (var i = 0; i < L; i++) {
          _areas.Add(new Area() {
            warehouse_id = warehouse.id,
            number = $"AN{i}",
            name = $"test area name {i}",
            comment = $"test area comment {i}",
            total_area = $"{i}",
          });
        }
      });

      _areas.UnitOfWork.SaveChanges();
    }

    private void InsertLocations()
    {
      _areas.Table.OrderBy(item => item.id).ToList().ForEach(area => {
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          _locations.Add(new Location() {
            area_id = area.id,
            total_area = $"{i}",
            total_volume = $"{i * i}",
            warehouse_id = area.warehouse_id,
            name = $"test location name {i}",
            number = $"LN{area.id}.{i}",
            comment = $"test location comment {i}",
          });
        }
      });

      _locations.UnitOfWork.SaveChanges();
    }

    private void InsertProjects()
    {
      foreach (var warehouse in _warehouses.All()) {
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          var now = DateTime.Now;
          var startAt = _random.DateTime(now.AddDays(-30), now.AddDays(30));
          var dueTime = _random.DateTime(startAt.AddDays(30), startAt.AddDays(180));
          var finishedAt = _random.Bool() ? _random.DateTime(dueTime.AddDays(-60), dueTime.AddDays(60)) : DateTime.MinValue;

          _projects.Add(new Project {
            warehouse_id = warehouse.id,
            number = $"PJ{i}",
            name = $"test project {i}",
            comment = $"test project comment {i}",
            due_time = dueTime,
            started_at = startAt,
            finished_at = finishedAt,
          });
        }
      }

      _projects.UnitOfWork.SaveChanges();
    }

    private void InsertItemCategories()
    {
      foreach (var warehouse in _warehouses.All()) {
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          _itemCategories.Add(new ItemCategory {
            warehouse_id = warehouse.id,
            name = $"test item category {i}",
            comment = $"test item category comment {i}",
          });
        }
      }

      _itemCategories.UnitOfWork.SaveChanges();
    }

    private void InsertOrderCategories()
    {
      foreach (var warehouse in _warehouses.All()) {
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          _orderCategories.Add(new OrderCategory {
            warehouse_id = warehouse.id,
            type = $"order.inbound",
            name = $"test item category {i}",
            comment = $"test item category comment {i}",
          });
        }
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          _orderCategories.Add(new OrderCategory {
            warehouse_id = warehouse.id,
            type = $"order.outbound",
            name = $"test item category {i}",
            comment = $"test item category comment {i}",
          });
        }
      }

      _orderCategories.UnitOfWork.SaveChanges();
    }

    private void InsertItems()
    {
      foreach (var warehouse in _warehouses.All()) {
        for (int i = 0, L = _random.Int(10, 30); i < L; i++) {
          _items.Add(new Item {
            warehouse_id = warehouse.id,
            number = $"item_000{i}",
            category_ids = new int[] {},
            name = $"test item {i}",
            specification = "个",
            comment = $"test item category comment {i}"
          });
        }
      }

      _items.UnitOfWork.SaveChanges();
    }

    private void InsertStocks()
    {
      foreach (var warehouse in _warehouses.All()) {
        var items = _items.Search(warehouse.id);
        var locations = _locations.Search(warehouse.id);
        items = _random.Array(items, _random.Int(5, 10));

        foreach (var item in items) {
          locations = _random.Array(locations, _random.Int(1, 10));
          foreach (var location in locations) {
            _stocks.Add(new Stock {
              warehouse_id = warehouse.id,
              item_id = item.id,
              location_id = location.id,
              quantity = _random.Int(10, 100)
            });
          }
        }
      }

      _items.UnitOfWork.SaveChanges();
    }

    private void InsertSuppliers()
    {
      foreach (var warehouse in _warehouses.All()) {
        for (int i = 0, L = _random.Int(10, 20); i < L; i++) {
          _suppliers.Add(new Supplier {
            warehouse_id = warehouse.id,
            name = $"test supplier {i}",
            comment = $"test supplier comment {i}",
          });
        }
      }

      _suppliers.UnitOfWork.SaveChanges();
    }

    private void InsertStockRecords()
    {
      // wait for orders insert
    }
  }
}

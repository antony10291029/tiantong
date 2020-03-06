using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Renet.Web;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class AppController : BaseController
  {
    private IAuth _auth;

    private DbContext _db;

    private IRandom _random;

    private IConfiguration _config;

    private UserRepository _users;

    private AreaRepository _areas;

    private ProjectRepository _projects;

    private LocationRepository _locations;

    private WarehouseRepository _warehouses;

    private ItemCategoryRepository _itemCategories;

    private OrderCategoryRepository _orderCategories;

    public AppController(
      IAuth auth,
      DbContext db,
      IRandom random,
      IConfiguration config,
      UserRepository users,
      AreaRepository areas,
      ProjectRepository projects,
      LocationRepository locations,
      WarehouseRepository warehouses,
      ItemCategoryRepository itemCategories,
      OrderCategoryRepository orderCategories
    ) {
      _db = db;
      _auth = auth;
      _users = users;
      _areas = areas;
      _random = random;
      _config = config;
      _projects = projects;
      _locations = locations;
      _warehouses = warehouses;
      _itemCategories = itemCategories;
      _orderCategories = orderCategories;
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
        return JsonMessage("system has been initialized");
      }

      InitializeRootUser();

      return JsonMessage("Success to initialize root user");
    }

    public object Restore()
    {
      _auth.EnsureRoot();
      var migrator = new PostgresMigrator();
      migrator.UseDbContext(_db);
      migrator.Refresh();

      return JsonMessage("Success to restore application");
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

    public object InsertTestData()
    {
      if (IsInitialized()) {
        return JsonMessage("System has been initialized");
      }

      InsertRootUser();
      InsertOwnerUsers();
      InsertWarehouses();
      InsertAreas();
      InsertLocations();
      InsertProjects();
      InsertItemCategories();
      InsertOrderCategories();

      return JsonMessage("Success to insert test data");
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
      foreach (var warehouse in _warehouses.Table.OrderBy(item => item.id).ToArray()) {
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          var now = DateTime.Now;
          var startAt = _random.DateTime(now.AddDays(-30), now.AddDays(30));
          var deadline = _random.DateTime(startAt.AddDays(30), startAt.AddDays(180));
          var finishedAt = _random.Bool() ? _random.DateTime(deadline.AddDays(-60), deadline.AddDays(60)) : DateTime.MinValue;

          _projects.Add(new Project {
            warehouse_id = warehouse.id,
            number = $"PJ{i}",
            name = $"test project {i}",
            comment = $"test project comment {i}",
            deadline = deadline,
            started_at = startAt,
            finished_at = finishedAt,
          });
        }
      }

      _projects.UnitOfWork.SaveChanges();
    }

    private void InsertItemCategories()
    {
      foreach (var warehouse in _warehouses.Table.OrderBy(item => item.id).ToArray()) {
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
      foreach (var warehouse in _warehouses.Table.OrderBy(item => item.id).ToArray()) {
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          _orderCategories.Add(new OrderCategory {
            warehouse_id = warehouse.id,
            type = $"order.in",
            name = $"test item category {i}",
            comment = $"test item category comment {i}",
          });
        }
        for (int i = 0, L = _random.Int(5, 10); i < L; i++) {
          _orderCategories.Add(new OrderCategory {
            warehouse_id = warehouse.id,
            type = $"order.out",
            name = $"test item category {i}",
            comment = $"test item category comment {i}",
          });
        }
      }

      _orderCategories.UnitOfWork.SaveChanges();
    }
  }
}

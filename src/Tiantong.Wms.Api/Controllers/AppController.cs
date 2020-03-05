using System.Collections.Generic;
using Renet.Web;
using Microsoft.Extensions.Configuration;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class AppController : BaseController
  {
    private IAuth _auth;

    private DbContext _db;

    private IConfiguration _config;

    private UserRepository _users;

    private WarehouseRepository _warehouses;

    public AppController(
      IAuth auth,
      DbContext db,
      IConfiguration config,
      UserRepository users,
      WarehouseRepository warehouses
    ) {
      _db = db;
      _auth = auth;
      _users = users;
      _config = config;
      _warehouses = warehouses;
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
      for (var i = 0; i < 100; i++) {
        var user = new User {
          type = UserTypes.Owner,
          password = "123456",
          email = "owner" + (i == 0 ? "" : i.ToString()) + "@wms.com"
        };

        _users.Add(user);
      }

      _users.UnitOfWork.SaveChanges();
    }
  }
}

using Tiantong.Wms.DB;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class DbContext : PostgresContext, IUnitOfWork
  {
    public DbSet<User> Users { get; set; }

    public DbSet<Keeper> Keepers { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

  }
}

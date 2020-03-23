using Tiantong.Wms.DB;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.Api
{
  public class DbContext : PostgresContext, IUnitOfWork
  {
    public DbSet<User> Users { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

    public DbSet<Keeper> Keepers { get; set; }

    public DbSet<Area> Areas { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<ItemCategory> ItemCategories { get; set; }

    public DbSet<OrderCategory> OrderCategories { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Stock> Stocks { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<StockRecord> StockRecords { get; set; }

    public DbSet<ProjectItem> ProjectItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
  }
}

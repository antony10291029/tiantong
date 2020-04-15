using System;
using Microsoft.EntityFrameworkCore;
using DBCore.Postgres;
using Tiantong.Wms.DB;

namespace Tiantong.Wms.Api
{
  public class DbContext : PostgresContext, IUnitOfWork
  {
    public DbSet<User> Users { get; set; }

    public DbSet<Good> Goods { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Stock> Stocks { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

    public DbSet<WarehouseUser> WarehouseUsers { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<DepartmentUser> DepartmentUsers { get; set; }

    public DbSet<Area> Areas { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<GoodCategory> GoodCategories { get; set; }

    public DbSet<StockRecord> StockRecords { get; set; }

    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public DbSet<PurchasePayment> PurchasePayments { get; set; }

    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public DbSet<PurchaseOrderItemProject> PurchaseItemProjects { get; set; }

    public DbContext(PostgresBuilder builder): base(builder)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {

    }
  }
}

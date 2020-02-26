using DBCore.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Tiantong.Wms.DB
{
  public class PostgresContext : DBCore.DbContext
  {
    public DbSet<User> Users { get; set; }

    public DbSet<Repository> Repositories { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<ItemStorageRecord> ItemStorageRecords { get; set; }

    public DbSet<ItemOutgoingRecord> ItemOutgoingRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      var builder = new PostgresBuilder();

      builder.Host("localhost").Port(5432).Database("wms").Password("123456");
      builder.OnConfiguring(options);
    }
  }
}

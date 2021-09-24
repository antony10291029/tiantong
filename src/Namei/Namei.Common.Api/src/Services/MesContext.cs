using Microsoft.EntityFrameworkCore;

namespace Namei.Common.Api
{
  public class MesContext: DbContext
  {
    private readonly Config _config;

    public DbSet<MesItemWarehouseInventory> MesItemWarehouseInventory { get; set; }

    public MesContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseSqlServer(_config.MesDatabase);
    }
  }
}

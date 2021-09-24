using Microsoft.EntityFrameworkCore;
using Namei.Common.Entities;
using System.IO;
using System.Threading.Tasks;

namespace Namei.Common.Api
{
  public class WmsContext: DbContext
  {
    private Config _config;

    public DbSet<WmsAsn> Asns { get; private set; }

    public DbSet<WmsAsnDetail> AsnDetails { get; private set; }

    public DbSet<WmsItem> Items { get; private set; }

    public DbSet<WmsTask> Tasks { get; private set; }

    public DbSet<WmsLocation> Locations { get; private set; }

    public DbSet<WmsMoveDoc> MoveDocs { get; private set; }

    public DbSet<WmsPickTicket> PickTickets { get; private set; }

    public DbSet<WmsInventory> Inventories { get; private set; }

    public DbSet<WmsItemKey> ItemKeys { get; private set; }

    public DbSet<WmsPickTicketTask> PickTicketTasks { get; private set; }

    public DbSet<WmsInventoryRestQuantity> InventoryRestQuantities { get; private set; }

    public DbSet<WmsItemWarehouseInventory> ItemWarehouseInventory { get; set; }

    public WmsContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseMySQL(_config.WmsUrl);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {

    }
  }
}

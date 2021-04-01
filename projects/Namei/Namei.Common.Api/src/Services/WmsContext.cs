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

    public WmsContext(Config config)
    {
      _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
      options.UseMySQL(_config.WmsUrl);
      options.LogTo(_logStream.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
      model.Entity<WmsAsn>()
        .HasMany(asn => asn.PickTickets)
        .WithOne()
        .HasForeignKey(ticket => ticket.RelatedBill1)
        .HasPrincipalKey(asn => asn.CUSTOMER_BILL);

      model.Entity<WmsPickTicket>()
        .HasMany(ticket => ticket.MoveDocs)
        .WithOne()
        .HasForeignKey(moveDoc => moveDoc.RelatedBillId)
        .HasPrincipalKey(ticket => ticket.Id);

      model.Entity<WmsMoveDoc>()
        .HasMany(moveDoc => moveDoc.Tasks)
        .WithOne()
        .HasForeignKey(task => task.MoveDocId)
        .HasPrincipalKey(moveDoc => moveDoc.Id);
    }

private readonly StreamWriter _logStream = new StreamWriter("sql.txt", append: true);

public override void Dispose()
{
    base.Dispose();
    _logStream.Dispose();
}

public override async ValueTask DisposeAsync()
{
    await base.DisposeAsync();
    await _logStream.DisposeAsync();
}
  }
}

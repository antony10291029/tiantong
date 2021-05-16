using Microsoft.EntityFrameworkCore;

namespace Midos.SeedWork
{
  public class EFContext: DbContext
  {
    private readonly EFContextOptions<EFContext> _options;

    public EFContext(EFContextOptions<EFContext> options)
    {
      _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      _options.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      _options.OnModelCreating(builder);
    }
  }
}

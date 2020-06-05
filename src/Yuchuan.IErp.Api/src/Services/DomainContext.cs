using Yuchuan.IErp.Database;
using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Api
{
  public class DomainContext: PostgresContext
  {
    public DbSet<SubjectCategory> SubjectCategories { get; set; }

    public DbSet<SubjectSubCategory> SubjectSubCategories { get; set; }

    public DomainContext(DbBuilder builder): base(builder)
    {

    }

  }
}
using Microsoft.EntityFrameworkCore;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class AppSystemContext: SystemContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseSqlite("Filename=./Data/log.db");
    }
  }

  public class AppLogContext: LogContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseSqlite("Filename=./Data/system.db");
    }
  }
}

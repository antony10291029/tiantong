using Microsoft.EntityFrameworkCore;

namespace Tiantong.Iot.Entities
{
  public class LogContext: DBCore.DbContext
  {
    public DbSet<EmailVerifyCode> EmailVerifyCodes { get; set; }

    public DbSet<HttpPusherError> HttpPusherErrors { get; set; }

    public DbSet<HttpPusherLog> HttpPusherLogs { get; set; }

    public DbSet<PlcLog> PlcLogs { get; set; }

    public DbSet<PlcStateError> PlcStateErrors { get; set; }

    public DbSet<PlcStateLog> PlcStateLogs { get; set; }

  }

}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Midos.Domain;

namespace Namei.Wcs.Api
{
  [Table("logs")]
  public class Log: IEntity
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    // todo rename
    [Column("type")]
    public string Level { get; private set; }

    [Column("class")]
    public string Class { get; private set; }

    [Column("operation")]
    public string Operation { get; private set; }

    [Column("index")]
    public string Index { get; private set; }

    [Column("data")]
    public string Data { get; private set; }

    [Column("message")]
    public string Message { get; private set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; }

    private Log() {}

    public static Log From(params Action<Log>[] hooks)
    {
      var log = new Log();

      log.CreatedAt = DateTime.Now;

      foreach (var hook in hooks) {
        hook(log);
      }

      return log;
    }

    public void UseHooks(params Action<Log>[] hooks)
    {
      foreach (var hook in hooks) {
        hook(this);
      }
    }

    public static Action<Log> UseSuccess()
      => log => log.Level = LogLevel.Success;

    public static Action<Log> UseInfo()
      => log => log.Level = LogLevel.Info;

    public static Action<Log> UseLink()
      => log => log.Level = LogLevel.Link;

    public static Action<Log> UseWarning()
      => log => log.Level = LogLevel.Warning;

    public static Action<Log> UseDanger()
      => log => log.Level = LogLevel.Danger;

    public static Action<Log> UseClass(string klass)
      => log => log.Class = klass;

    public static Action<Log> UseOperation(string operation)
      => log => log.Operation = operation;

    public static Action<Log> UseIndex(string index)
      => log => log.Index = index;

    public static Action<Log> UseData(string data)
      => log => log.Data = data;

    public static Action<Log> UseData(object data)
      => log => log.Data = System.Text.Json.JsonSerializer.Serialize(data ?? new {});

    public static Action<Log> UseMessage(string message)
      => log => log.Message = message;
  }
}

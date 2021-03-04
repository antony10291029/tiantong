using System.Data;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Wcs.Api
{
  [Table("logs")]
  public class Log
  {
    [Key]
    [Column("id")]
    public int Id { get; private set; }

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

    // remove
    [Column("key")]
    public string Key { get; private set; }

    [Column("message")]
    public string Message { get; private set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; }

    private Log()
    {

    }

    public static Log From(
      string level,
      string klass,
      string operation,
      string index,
      string message,
      string data
    ) {
      return new Log() {
        Level = level,
        Class = klass,
        Index = index,
        Operation = operation,
        Data = data,
        Message = message,
        CreatedAt = DateTime.Now,

        Key = "old"
      };
    }

    public static Log FromOld(string key, string level, string message)
    {
      return new Log() {
        Class = "old",
        Index = "old",
        Operation = "old",
        Data = "old",
        Level = level,
        Key = key,
        Message = message,
        CreatedAt = DateTime.Now,
      };
    }

    public Log UseOperator(string klass, string operation, string index)
    {
      Class = klass;
      Operation = operation;
      Index = index;

      return this;
    }

    // todo remove
    public Log UseKey(string key)
    {
      Key = key;

      return this;
    }

    public Log UseSuccess(string level)
    {
      Level = LogLevel.Success;

      return this;
    }

    public Log UseInfo(string level)
    {
      Level = LogLevel.Info;

      return this;
    }

    public Log UseLink(string level)
    {
      Level = LogLevel.Link;

      return this;
    }

    public Log UseWarning(string level)
    {
      Level = LogLevel.Warning;

      return this;
    }

    public Log UseDanger(string level)
    {
      Level = LogLevel.Danger;

      return this;
    }
  }
}

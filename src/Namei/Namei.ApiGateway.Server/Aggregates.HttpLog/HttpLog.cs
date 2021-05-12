using Microsoft.EntityFrameworkCore;
using Midos.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Namei.ApiGateway.Server
{
  [Index(nameof(RequestUri))]
  [Index(nameof(SourcePath))]
  public class HttpLog: IEntity
  {
    public long Id { get; set; }

    public string SourcePath { get; set; }

    public string RequestMethod { get; set; }

    public string RequestUri { get; set; }

    public string RequestVersion { get; set; }

    [Column("RequestHeaders")]
    public string InnerRequestHeaders { get; set; }

    public string RequestBody { get; set; }

    public string ResponseVersion { get; set; }

    public string ResponseStatus { get; set; }

    [JsonIgnore]
    [Column("ResponseHeaders")]
    public string InnerResponseHeaders { get; set; }

    public string ResponseBody { get; set; }

    [JsonIgnore]
    [Column("Exception")]
    public string InnerException { get; set; }

    [NotMapped]
    public object RequestHeaders
    {
      get => JsonSerializer.Deserialize<object>(InnerRequestHeaders ?? "null");
      set => InnerRequestHeaders = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public object ResponseHeaders
    {
      get => JsonSerializer.Deserialize<object>(InnerResponseHeaders ?? "null");
      set => InnerResponseHeaders = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public object Exception
    {
      get => JsonSerializer.Deserialize<object>(InnerException ?? "null");
      set => InnerException = JsonSerializer.Serialize(ConvertException(value as Exception));
    }

    public DateTime RequestedAt { get; set; }

    public DateTime ResponsedAt { get; set; }

    private static object ConvertException(Exception exception, int deep = 0)
      => exception is null ? null : new {
        exception.Data,
        exception.Message,
        exception.Source,
        exception.HResult,
        exception.HelpLink,
        StackTrace = exception.StackTrace?.Split('\n').Select(row => row.Trim()),
        InnerException = deep > 10 ? null : ConvertException(exception.InnerException, deep + 1)
      };
  }
}

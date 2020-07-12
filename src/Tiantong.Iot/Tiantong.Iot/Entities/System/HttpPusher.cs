using Renet.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("http_pushers")]
  public class HttpPusher
  {
    [Key]
    public virtual int id { get; set; }

    public virtual string name { get; set; }

    [MaxLength(20, ErrorMessage = "编号长度不可超过20")]
    public virtual string number { get; set; }


    [Url(ErrorMessage = "URL 格式错误")]
    public virtual string url { get; set; }

    public virtual string when_opt { get; set; }

    public virtual string when_value { get; set; }

    public virtual string value_key { get; set; }

    public virtual bool is_value_to_string { get; set; }

    [JsonObject]
    public virtual string data { get; set; }

    public virtual bool is_concurrent { get; set; }
  }
}

using Midos.Center.Controllers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Midos.Center.Entities
{
  [Table("apps")]
  public class App
  {
    [Key]
    [Column("id")]
    public long Id { get; private set; }

    [Column("class")]
    public string Class { get; private set; }

    [Column("key")]
    public string Key { get; private set; }

    [Column("name")]
    public string Name { get; private set; }

    [Column("url")]
    public string Url { get; private set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private set; }

    private App() {}

    public void Update(
      string key,
      string name,
      string url
    ) {
      Key = key;
      Name = name;
      Url = url;
    }

    public static App From(
      string klass,
      string key,
      string name,
      string url
    ) {
      return new App() {
        Class = klass,
        Key = key,
        Name = name,
        Url = url
      };
    }

  }
}

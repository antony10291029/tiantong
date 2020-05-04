using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiantong.Iot.Entities
{
  [Table("plc_connections")]
  public class PlcConnection
  {
    [Column("id")]
    [Key]
    public int Id { get; set; } = 0;

    [Column("model")]
    [Required]
    public string Model { get; set; } = "test";

    [Column("name")]
    [Required]
    public string Name { get; set; } = "default";

    [Column("host")]
    public string Host { get; set; } = "";

    [Column("port")]
    public int Port { get; set; } = 0;

    [Column("is_running")]
    public bool IsRunning { get; set; } = false;

    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }

    public PlcConnection()
    {
      CreatedAt = DateTime.Now;
    }
  }
}

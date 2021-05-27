using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;

namespace Namei.Wcs.Aggregates
{
  public static class TcsMainTaskStatus
  {
    public const string Finished = "9";

    public const string Started = "2";
  }

  [Table("tcs_main_task")]
  public class TcsMainTask
  {
    [Key]
    [Column("main_task_num")]
    public string MainTaskNum { get; set; }

    [Column("task_status")]
    public string TaskStatus { get; set; }

    [Column("via_codes")]
    public string ViaCodes { get; set; }

    [NotMapped]
    public string Destination
    {
      get {
        var codes = JsonSerializer.Deserialize<string[]>(ViaCodes);

        return codes.Length == 2 ? codes[1] : null;
      }
    }
  }
}

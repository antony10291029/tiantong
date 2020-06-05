using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("subject_sub_categories")]
  public class SubjectSubCategory
  {
    public int id { get; set; }

    public int category_id { get; set; }

    public string name { get; set; }

    public string subject_code { get; set; }
  }
}
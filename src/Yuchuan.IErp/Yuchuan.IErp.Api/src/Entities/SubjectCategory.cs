using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuchuan.IErp.Api
{
  [Table("subject_categories")]
  public class SubjectCategory
  {
    public int id { get; set; }

    public string name { get; set; }

    public string book_code { get; set; }

    [ForeignKey("category_id")]
    public List<SubjectSubCategory> sub_categories { get; set; }
  }
}
using System.ComponentModel.DataAnnotations;
namespace Renet.Web
{
  public abstract class BaseSearchParams
  {
    [Range(1, int.MaxValue)]
    public virtual int page { get; set; }


    [Range(1, int.MaxValue)]
    public virtual int page_size { get; set; }

    public virtual string search { get; set; }
  }
}

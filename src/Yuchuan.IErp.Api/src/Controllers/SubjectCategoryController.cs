using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Api
{
  [Route("/")]
  public class SubjectCategoryController: BaseController
  {
    private DomainContext _context;

    public SubjectCategoryController(DomainContext context)
    {
      _context = context;
    }

    [HttpPost]
    [Route("/subject-categories/create")]
    public object Create([FromBody] SubjectCategory category)
    {
      _context.Add(category);
      _context.SaveChanges();

      return SuccessOperation("分类已创建");
    }

    [HttpPost]
    [Route("/subject-categories/update")]
    public object Update([FromBody] SubjectCategory category)
    {
      _context.Update(category);
      _context.SaveChanges();

      return SuccessOperation("分类已更新");
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("/subject-categories/delete")]
    public object Delete([FromBody] DeleteParams param)
    {
      var category = _context.SubjectCategories.FirstOrDefault(sc => sc.id == param.id);
      _context.Remove(category);
      _context.SaveChanges();

      return SuccessOperation("分类已删除");
    }

    public class SearchParams
    {
      public string book_code { get; set; }
    }

    [HttpPost]
    [Route("/subject-categories/search")]
    public SubjectCategory[] Search([FromBody] SearchParams param)
    {
      return _context.SubjectCategories
        .Include(sc => sc.sub_categories)
        .Where(sc => sc.book_code == param.book_code)
        .ToArray();
    }
  }
}
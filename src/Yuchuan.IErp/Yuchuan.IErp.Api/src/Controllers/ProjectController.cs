using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renet;
using Renet.Web;
using System.Collections.Generic;
using System.Linq;

namespace Yuchuan.IErp.Api
{
  public class ProjectController: BaseController
  {
    private Auth _auth;

    private DomainContext _domain;

    public ProjectController(
      Auth auth,
      DomainContext domain
    ) {
      _auth = auth;
      _domain = domain;
    }

    [HttpPost]
    [Route("/projects/create")]
    public ActionResult<object> Create([FromBody] Project param)
    {
      var userId = _auth.GetUserId();

      param.Users = new List<ProjectUser>() {
        new ProjectUser {
          user_id = userId,
          role = ProjectUserType.Owner,
        }
      };

      param.type = ProjectType.Lifter;
      _domain.Add(param);
      _domain.SaveChanges();

      return new {
        message = "项目已创建",
        id = param.id
      };
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("/projects/delete")]
    public ActionResult<object> Remove([FromBody] DeleteParams param)
    {
      var userId = _auth.GetUserId();

      if (!_domain.ProjectUsers.Any(pu => pu.user_id == userId)) {
        throw KnownException.Error("无项目权限", 403);
      }

      if (_domain.ProjectDevices.Any(pd => pd.project_id == param.id)) {
        throw KnownException.Error("请先删除项目下的设备", 403);
      }

      var project = _domain.Projects
        .Include(p => p.Users)
        .FirstOrDefault(p => p.id == param.id);

      if (project == null) {
        throw KnownException.Error("项目不存在", 404);
      }

      _domain.Remove(project);
      _domain.SaveChanges();

      return new {
        message = "项目已删除"
      };
    }

    [HttpPost]
    [Route("/projects/update")]
    public ActionResult<object> Update([FromBody] Project param)
    {
      var userId = _auth.GetUserId();
      if (!_domain.ProjectUsers.Any(pu => pu.user_id == userId)) {
        throw KnownException.Error("无项目权限", 403);
      }

      var oldProject = _domain.Projects.FirstOrDefault(p => p.id == param.id);

      if (oldProject == null) {
        throw KnownException.Error("项目不存在", 404);
      }

      _domain.Entry(oldProject).CurrentValues.SetValues(param);
      _domain.Entry(oldProject).Property(p => p.type).IsModified = false;
      _domain.Entry(oldProject).Property(p => p.created_at).IsModified = false;
      oldProject.Users = null;
      oldProject.Devices = null;
      _domain.SaveChanges();

      return new {
        message = "项目已修改"
      };
    }

    public class SearchParams
    {

    }

    [HttpPost]
    [Route("/projects/search")]
    public ActionResult<object> Search([FromBody] SearchParams param)
    {
      var userId = _auth.GetUserId();
      var ids = _domain.ProjectUsers
        .Where(pu => pu.user_id == userId)
        .Select(p => p.project_id)
        .Distinct();

      return _domain.Projects
        .Where(p => ids.Contains(p.id))
        .OrderBy(p => p.number)
        .ToArray();
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("/projects/find")]
    public ActionResult<object> Find([FromBody] FindParams param)
    {
      var userId = _auth.GetUserId();
      var project = _domain.Projects.FirstOrDefault(p => p.id == param.id);

      if (project is null) {
        throw KnownException.Error("项目不存在", 404);
      }

      if (!_domain.ProjectUsers.Any(p =>
        p.user_id == userId && p.project_id == param.id
      )) {
        throw KnownException.Error("无项目权限", 403);
      }

      return project;
    }
    
  }
}

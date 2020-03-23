using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class ProjectController : BaseController
  {
    private IAuth _auth;

    private ProjectRepository _projects;

    private WarehouseRepository _warehouses;

    private ProjectItemRepository _projectItems;

    public ProjectController(
      IAuth auth,
      ProjectRepository projects,
      WarehouseRepository warehouses,
      ProjectItemRepository projectItems
    ) {
      _auth = auth;
      _projects = projects;
      _warehouses = warehouses;
      _projectItems = projectItems;
    }

    public class CreateParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      [Required]
      public string number { get; set; }

      public string name { get; set; } = "";

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;

      public DateTime due_time { get; set; } = DateTime.Now;

      public DateTime started_at { get; set; } = DateTime.Now;

      public DateTime finished_at { get; set; } = DateTime.MinValue;
    }

    public object Create([FromBody] CreateParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _projects.EnsureNumberUnique(param.warehouse_id, param.number);

      var project = new Project {
        warehouse_id = param.warehouse_id,
        number = param.number,
        name = param.name,
        comment = param.comment,
        is_enabled = param.is_enabled,
        due_time = param.due_time,
        started_at = param.started_at,
        finished_at = param.finished_at
      };
      _projects.Add(project);
      _projects.UnitOfWork.SaveChanges();

      return SuccessOperation("工程已创建", project.id);
    }

    public class DeleteParams
    {
      [Nonzero]
      public int id { get; set; }
    }

    public object Delete([FromBody] DeleteParams param)
    {
      _auth.EnsureOwner();

      var project = _projects.EnsureGetByOwner(param.id, _auth.User.id);
      if (_projectItems.HasProject(project.warehouse_id, project.id)) {
        return FailureOperation("工程已使用，无法删除");
      }
      _projects.Remove(project.id);
      _projects.UnitOfWork.SaveChanges();

      return SuccessOperation("工程已删除");
    }

    public class ProjectUpdateParams
    {
      [Nonzero]
      public int id { get; set; }

      public string number { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }

      public DateTime? due_time { get; set; }

      public DateTime? started_at { get; set; }

      public DateTime? finished_at { get; set; }
    }

    public object Update([FromBody] ProjectUpdateParams param)
    {
      _auth.EnsureOwner();
      var project = _projects.EnsureGetByOwner(param.id, _auth.User.id);

      if (param.name != null) project.name = param.name;
      if (param.comment != null) project.comment = param.comment;
      if (param.is_enabled != null) {
        project.is_enabled = (bool) param.is_enabled;
      }
      if (param.due_time != null) {
        project.due_time = (DateTime) param.due_time;
      }
      if (param.started_at != null) {
        project.started_at = (DateTime) param.started_at;
      }
      if (param.finished_at != null) {
        project.finished_at = (DateTime) param.finished_at;
      }
      if (param.number != null) {
        _projects.EnsureNumberUnique(project.warehouse_id, param.number);
        project.number = param.number;
      }
      _projects.UnitOfWork.SaveChanges();

      return SuccessOperation("工程信息已保存");
    }

    public class SearchParams: BaseSearchParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }
    }

    public IPagination<Project> Search([FromBody] SearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      return _projects.Table
        .Where(project =>
          project.warehouse_id == param.warehouse_id &&
          (
            param.search == null ? true :
            project.name.Contains(param.search) ||
            project.number.Contains(param.search)
          )
        )
        .OrderBy(project => project.number)
        .ThenBy(project => project.id)
        .Paginate(param.page, param.page_size);
    }

    public class FindParams
    {
      [Nonzero]
      public int warehouse_id { get; set; }

      [Nonzero]
      public int project_id { get; set; }
    }

    public Project Find([FromBody] FindParams param)
    {
      _auth.EnsureOwner();

      return _projects.EnsureGet(param.project_id, param.warehouse_id, _auth.User.id);
    }
  }
}

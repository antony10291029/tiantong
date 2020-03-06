using System;
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

    public ProjectController(
      IAuth auth,
      ProjectRepository projects,
      WarehouseRepository warehouses
    ) {
      _auth = auth;
      _projects = projects;
      _warehouses = warehouses;
    }

    public class ProjectCreateParams
    {
      [Required]
      public int warehouse_id { get; set; }

      [Required]
      public string number { get; set; }

      public string name { get; set; } = "";

      public string comment { get; set; } = "";

      public bool is_enabled { get; set; } = true;

      public DateTime deadline { get; set; } = DateTime.Now;

      public DateTime started_at { get; set; } = DateTime.Now;

      public DateTime finished_at { get; set; } = DateTime.MinValue;
    }

    public object Create([FromBody] ProjectCreateParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);
      _projects.EnsureNumberUnique(param.warehouse_id, param.number);

      _projects.Add(new Project() {
        warehouse_id = param.warehouse_id,
        number = param.number,
        name = param.name,
        comment = param.comment,
        is_enabled = param.is_enabled,
        started_at = param.started_at,
        finished_at = param.finished_at
      });
      _projects.UnitOfWork.SaveChanges();

      return JsonMessage("Success to create project");
    }

    public class ProjectDeleteParams
    {
      public int id { get; set; }
    }

    public object Delete([FromBody] ProjectDeleteParams param)
    {
      _auth.EnsureOwner();
      var project =  _projects.EnsureGetByOwner(param.id, _auth.User.id);
      _projects.Remove(param.id);
      _projects.UnitOfWork.SaveChanges();

      return JsonMessage("Success to delete project");
    }

    public class ProjectUpdateParams
    {
      [Required]
      public int? id { get; set; }

      [Required]
      public string number { get; set; }

      public string name { get; set; }

      public string comment { get; set; }

      public bool? is_enabled { get; set; }

      public DateTime? deadline { get; set; }

      public DateTime? started_at { get; set; }

      public DateTime? finished_at { get; set; }
    }

    public object Update([FromBody] ProjectUpdateParams param)
    {
      _auth.EnsureOwner();
      var project = _projects.EnsureGetByOwner((int) param.id, _auth.User.id);

      if (param.name != null) project.name = param.name;
      if (param.comment != null) project.comment = param.comment;
      if (param.is_enabled != null) {
        project.is_enabled = (bool) param.is_enabled;
      }
      if (param.deadline != null) {
        project.deadline = (DateTime) param.deadline;
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

      return JsonMessage("Success to update project");
    }

    public class ProjectSearchParams
    {
      public int warehouse_id { get; set; }
    }

    public Project[] Search([FromBody] ProjectSearchParams param)
    {
      _auth.EnsureOwner();
      _warehouses.EnsureOwner(param.warehouse_id, _auth.User.id);

      return _projects.Search(param.warehouse_id);
    }
  }
}

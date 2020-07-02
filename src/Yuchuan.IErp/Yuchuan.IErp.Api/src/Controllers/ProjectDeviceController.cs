using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Yuchuan.IErp.Api
{
  public class ProjectDeviceController: BaseController
  {
    private Auth _auth;

    private DomainContext _domain;

    public ProjectDeviceController(
      Auth auth,
      DomainContext domain
    ) {
      _auth = auth;
      _domain = domain;
    }

    private void EnsureProjectUser(int projectId)
    {
      var userId = _auth.GetUserId();
      var project = _domain.Projects.FirstOrDefault(p => p.id == projectId);
      var pu = _domain.ProjectUsers.FirstOrDefault(pu =>
        pu.project_id == projectId && pu.user_id == userId
      );

      if (project is null) {
        throw KnownException.Error("项目不存在", 404);
      }
      if (pu is null) {
        throw KnownException.Error("无项目权限", 403);
      }
    }

    [HttpPost]
    [Route("/devices/create")]
    public ActionResult<object> Create([FromBody] ProjectDevice param)
    {
      EnsureProjectUser(param.project_id);

      param.device.state = new List<DeviceState> {
        new DeviceState {}
      };

      _domain.Add(param);
      _domain.SaveChanges();

      return new {
        message = "设备已创建",
        id = param.device.id,
      };
    }

    public class DeleteParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("/devices/delete")]
    public ActionResult<object> Delete([FromBody] DeleteParams param)
    {
      var pd = _domain.ProjectDevices
        .Include(pd => pd.device)
        .FirstOrDefault(pd => pd.device_id == param.id);

      if (pd is null) {
        throw KnownException.Error("设备不存在", 404);
      }

      EnsureProjectUser(pd.project_id);

      _domain.Remove(pd);
      _domain.SaveChanges();

      return new {
        message = "设备已删除"
      };
    }

    [HttpPost]
    [Route("/devices/update")]
    public ActionResult<object> Update([FromBody] Device param)
    {
      var pd = _domain.ProjectDevices
        .Include(pd => pd.device)
        .FirstOrDefault(pd => pd.device_id == param.id);

      EnsureProjectUser(pd.project_id);

      if (pd == null) {
        throw KnownException.Error("设备不存在", 404);
      }

      var device = pd.device;

      _domain.Entry(device).CurrentValues.SetValues(param);
      _domain.SaveChanges();

      return new {
        message = "设备已更新"
      };
    }

    public class SearchParams
    {
      public int project_id { get; set; }
    }

    [HttpPost]
    [Route("/devices/search")]
    public ActionResult<object> Update([FromBody] SearchParams param)
    {
      EnsureProjectUser(param.project_id);

      var ids = _domain.ProjectDevices
        .Where(pd => pd.project_id == param.project_id)
        .Select(pd => pd.id);
      var devices = _domain.Devices
        .Where(d => ids.Contains(d.id))
        .OrderBy(d => d.number)
          .ThenBy(d => d.id)
        .ToArray();

      return devices;
    }

    public class FindParams
    {
      public int id { get; set; }
    }

    [HttpPost]
    [Route("/devices/find")]
    public ActionResult<object> Find([FromBody] FindParams param)
    {
      var userId = _auth.GetUserId();
      var pd = _domain.ProjectDevices
        .Include(pd => pd.device)
        .FirstOrDefault(pd => pd.device_id == param.id);

      if (pd is null) {
        throw KnownException.Error("设备不存在", 404);
      }

      EnsureProjectUser(pd.project_id);

      return pd.device;
    }
  }
}

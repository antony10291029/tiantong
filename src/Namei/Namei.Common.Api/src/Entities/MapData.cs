using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Api
{
  // 参考 rcs 数据库
  [Table("tcs_map_data")]
  public class MapData
  {
    // 地图数据编号（地图编号 + X 坐标 + Y 坐标）
    [Key]
    [Column("map_data_code")]
    public string MapDataCode { get; set; }

    // X 坐标
    [Column("coo_x")]
    public float CooX { get; set; }

    // Y 坐标
    [Column("coo_y")]
    public float CooY { get; set; }

    // 方向
    // [Column("dir")]
    // public string Dir { get; set; }

    // 状态，1 启用，0 禁用
    [Column("status")]
    public int status { get; set; }

    // 电子地图编号
    [Column("map_code")]
    public string MapCode { get; set; }

    // 组织编号
    // [Column("org_code")]
    // public string OrgCode { get; set; }

    // 地图数据名称
    [Column("data_name")]
    public string DataName { get; set; }

    // 地图元素名称
    [Column("data_typ")]
    public string DataType { get; set; }

    // 容器编号
    [Column("pod_code")]
    public string PodCode { get; set; }

    // 锁定标记，0 未锁定
    // [Column("ind_lock")]
    // public int IndLock { get; set; }

    // 锁定源
    // [Column("lock_source")]
    // public string LockSource { get; set; }

    // 创建时间
    // [Column("date_cr")]
    // public DateTime CreatedAt { get; set; }

    // 更新时间
    // [Column("date_chg")]
    // public DateTime UpdatedAt { get; set; }

    // 关联储位编号
    // [Column("linked_code")]
    // public string LinkedCode { get; set; }

    // 关联储位编号
    // [Column("berth_typ")]
    // public string BerthType { get; set; }

    // 备注
    // [Column("remark")]
    // public string Remark { get; set; }

    // 区域编号
    [Column("area_code")]
    public string AreaCode { get; set; }

    // 节拍编号
    // [Column("map_ele_param_conf_id")]
    // public string MapEleParamConfId { get; set; }

    // 存储区编号
    // [Column("stg_sec_code")]
    // public string StgSecCode { get; set; }

    // 举升高度（出库为起点，转移为起点，回库为重点
    // [Column("lift_height")]
    // public string LiftHeight { get; set; }

    // 关联点编号
    // [Column("relate_position_code")]
    // public string RelatedPositionCode { get; set; }

    // 关联区域编号
    // [Column("relate_area_code")]
    // public string RelatedAreaCode { get; set; }

    // 关联货架类型
    // [Column("relate_pod_typ")]
    // public string RelatedPodType { get; set; }

    // 设备类型编号
    // [Column("device_type_code")]
    // public string DeviceTypeCode { get; set; }

    // 充电桩支持的车型
    // [Column("chargedevicetype")]
    // public string ChargeDeviceType { get; set; }

    // 小车进入方向
    // [Column("robotdir")]
    // public string RobotDir { get; set; }

    // 是否在巷道内
    // [Column("in_road_way")]
    // public int? InRoadWay { get; set; }

    // 巷道内起点
    // [Column("road_way_start_point")]
    // public string RoadWayStartPoint { get; set; }

    // 巷道内终点
    // [Column("road_way_stop_point")]
    // public string RoadWayStopPoint { get; set; }

    // 关联任务
    // [Column("main_task_typ_code")]
    // public string MainTaskTypeCode { get; set; }

    // 货架方向
    // [Column("pod_dir")]
    // public string PodDir { get; set; }
  }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Open.Server
{
  [Table("wms_box_code_bind")]
  public class WmsBoxCodeBind
  {
    [Column("ID")]
    public long Id { get; set; }

    [Column("BE_DELETE")]
    public string BeDelete { get; set; }

    [Column("CREATED_TIME")]
    public DateTime CreatedTime { get; set; }

    [Column("CREATOR")]
    public string Creator { get; set; }

    [Column("CREATOR_UNIKEY")]
    public string CreatorUnikey { get; set; }

    [Column("LAST_OPERATOR")]
    public string LastOperator { get; set; }

    [Column("LAST_OPERATOR_UNIKEY")]
    public string LastOperatorUnikey { get; set; }

    [Column("UPDATE_TIME")]
    public string UpdateTime { get; set; }

    [Column("ORDER_CODE")]
    public string OrderCode { get; set; }

    [Column("BOX_CODE")]
    public string BoxCode { get; set; }

    [Column("ITEM_CODE")]
    public string ItemCode { get; set; }

    [Column("BIND_TIME")]
    public DateTime BindTime { get; set; }

    [Column("VERSION")]
    public long Version { get; set; }
  }
}

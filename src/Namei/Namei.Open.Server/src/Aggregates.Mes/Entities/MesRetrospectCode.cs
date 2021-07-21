using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Namei.Open.Server
{
  [Keyless]
  [Table("MesWoRetrospectCodeByTemp")]
  public class MesRetrospectCode
  {
    public string WoOrderNo { get; set; }

    public string MaterielLot { get; set; }

    public string ContainerID { get; set; }

    public string PalletID { get; set; }

    public string BoxBarCode { get; set; }

    public string ItemBarCode { get; set; }

    public DateTime CreateDateTime { get; set; }

    public DateTime UpdateDateTime { get; set; }

    [Column("guid")]
    public string Guid { get; set; }
  }
}

using Midos.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Namei.Common.Entities
{
  [Table("wms_pick_ticket_task")]
  public class WmsPickTicketTask: IEntity
  {
    public long Id { get; private set; }

    public string OrderNumber { get; private set; }

    public string ItemCode { get; private set; }

    public string ItemName { get; private set; }

    public string PalletCode { get; private set; }

    public string PickedQuantity { get; private set; }

    public string RestQuantity { get; private set; }

    public string FromName { get; private set; }

    public string LocationCode { get; private set; }

    public string Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
  }
}

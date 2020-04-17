namespace Tiantong.Wms.Api
{
  public class RequisitionOrderRepository : OrderRepository
  {
    public RequisitionOrderRepository(
      IAuth auth,
      DbContext db,
      GoodRepository goods,
      StockRepository stocks,
      ProjectRepository projects,
      LocationRepository locations,
      SupplierRepository suppliers,
      WarehouseRepository warehouses,
      DepartmentRepository departments,
      WarehouseUserRepository warehouseUsers
    ) : base(auth, db, goods, stocks, projects, locations, suppliers, warehouses, departments, warehouseUsers) {

    }

    protected override bool RequireDepartment { get; } = true;

    protected override bool RequireApplicant { get; } = true;

    public Order EnsureGet(int warehouseId, int orderId)
    {
      return EnsureGet(warehouseId, orderId, OrderType.Requisition);
    }

    public override Order Add(Order order)
    {
      return Add(order, OrderType.Requisition);
    }

    public void Remove(int warehouseId, int orderId)
    {
      Remove(warehouseId, orderId, OrderType.Requisition);
    }

    public override Order Update(Order order)
    {
      return Update(order, OrderType.Requisition);
    }

    public void Finish(int warehouseId, int orderId, int locationId)
    {
      Finish(warehouseId, orderId, locationId, OrderType.Requisition);
    }

    public void File(int warehouseId, int orderId)
    {
      File(warehouseId, orderId, OrderType.Requisition);
    }

    public void Restore(int warehouseId, int orderId)
    {
      Restore(warehouseId, orderId, OrderType.Requisition);
    }

  }
}

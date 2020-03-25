using DBCore;

namespace Tiantong.Wms.DB
{
  public class CreatePurchaseItemProjects : IMigration
  {
    public void Up(DbContext db)
    {
      db.ExecuteFromSql("Migration.0043_CreatePurchaseItemProjects");
    }

    public void Down(DbContext db)
    {
      db.ExecuteSql("drop table if exists purchase_item_projects");
    }
  }
}

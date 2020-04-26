namespace Tiantong.Wms.Api
{
  public class RootRepository: UserRepository
  {
    private Config _config;

    public RootRepository(IHash hash, DbContext db, Config config): base(db, hash)
    {
      _config = config;
    }

    public void Create()
    {
      var user = new User();

      user.name = "root";
      user.type = UserType.Root;
      user.email = _config.ROOT_EMAIL;
      user.password = _config.PG_PASSWORD;

      Add(user);
      UnitOfWork.SaveChanges();
    }
  }
}

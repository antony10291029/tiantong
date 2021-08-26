using System.Linq;
using Midos.App;

namespace Midos.Account.Api
{
  public class PolicyDomainService
  {
    private readonly AppContext _context;

    public PolicyDomainService(AppContext context)
    {
      _context = context;
    }

    public void AddRange(Policy[] entities)
    {
      var policies = Policy.FromEntities(entities);

      _context.AddRange(policies);
      _context.SaveChanges();
    }

    public void Clear()
    {
      var policies = _context.AsQueryable<Policy>().ToArray();

      _context.RemoveRange(policies);
      _context.SaveChanges();
    }
  }
}

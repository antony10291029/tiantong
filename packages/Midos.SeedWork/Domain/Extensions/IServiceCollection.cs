using Midos.SeedWork.Domain;
using Savorboard.CAP.InMemoryMessageQueue;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class IServiceCollectionExtensions
  {
    public static void AddEFContext<TEFContext, TEFContextOptions>(this IServiceCollection services)
      where TEFContext: EFContext
      where TEFContextOptions: EFContextOptions<TEFContext>
    {
      services.AddCap(cap => {
        cap.ConsumerThreadCount = 10;
        cap.FailedRetryCount = 0;
        cap.UseInMemoryStorage();
        cap.UseInMemoryMessageQueue();
        cap.UseDashboard();
      });

      services.AddDbContext<TEFContext>();
      services.AddDbContext<EFContext, TEFContext>();
      services.AddSingleton<TEFContextOptions>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
  }
}

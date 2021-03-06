using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace TTT.Foundation.DependencyInjection
{
    [ExcludeFromCodeCoverage]
  public class DiConfigurator : IServicesConfigurator
  {
    public void Configure(IServiceCollection serviceCollection)
    {
      serviceCollection.AddControllers<IHttpController>("*.Foundation.*");
      serviceCollection.AddClassesWithServiceAttribute("*.Foundation.*");

      serviceCollection.AddControllers<IHttpController>("*.Feature.*");
      serviceCollection.AddClassesWithServiceAttribute("*.Feature.*");
    }
  }
}
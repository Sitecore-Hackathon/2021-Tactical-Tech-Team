namespace TTT.Foundation.JssExtensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    using Sitecore.DependencyInjection;
    using Sitecore.JavaScriptServices.ViewEngine.Http;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Scoped<IHttpRenderEngineFactory, HttpRenderEngineFactory>());
        }
    }
}
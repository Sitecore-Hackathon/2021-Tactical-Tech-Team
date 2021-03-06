namespace TTT.Foundation.JssExtensions.Pipelines.MvcGetRenderer
{
    using System.Web;

    using Sitecore;
    using Sitecore.Abstractions;
    using Sitecore.JavaScriptServices.Configuration;
    using Sitecore.JavaScriptServices.Configuration.Extensions;
    using Sitecore.JavaScriptServices.ViewEngine.Presentation;
    using Sitecore.LayoutService.Configuration;
    using Sitecore.LayoutService.ItemRendering;
    using Sitecore.LayoutService.Serialization;
    using Sitecore.Mvc.Pipelines.Response.GetRenderer;
    using Sitecore.Mvc.Presentation;

    using TTT.Foundation.JssExtensions.Presentation;

    public class GetJsLayoutRenderer : Sitecore.JavaScriptServices.ViewEngine.Presentation.Pipelines.MvcGetRenderer.GetJsLayoutRenderer
    {
        public GetJsLayoutRenderer(
            ILayoutService layoutService,
            ISerializerService serializerService,
            IConfiguration layoutServiceConfiguration,
            IConfigurationResolver appConfigurationResolver,
            IJssRendererConfiguration jssRendererConfiguration,
            BaseCorePipelineManager pipelineManager)
            : base(layoutService, serializerService, layoutServiceConfiguration, appConfigurationResolver, jssRendererConfiguration, pipelineManager)
        {
        }

        protected override Renderer GetRenderer(GetRendererArgs args)
        {
            if (!Context.Site.EnableIntegratedRendering())
            {
                HttpContext.Current.Response.StatusCode = 404;
                HttpContext.Current.Server.Transfer(Context.Site.GetItemNotFoundUrl());
            }

            var appConfig = this.ResolveAppConfiguration(args.Rendering.Item);
            if (appConfig == null)
            {
                return new JssAppNotFoundStandardValuesRenderer();
            }

            var layoutServiceNamedConfig = this.ResolveNamedConfiguration(appConfig);
            return new NuxtJsLayoutRenderer(args.Rendering, appConfig, layoutServiceNamedConfig, this.LayoutService, this.SerializerService, this.JssRendererConfig);
        }
    }
}
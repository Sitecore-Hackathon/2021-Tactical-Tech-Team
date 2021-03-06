namespace TTT.Foundation.JssExtensions.Presentation
{
    using System;
    using System.IO;

    using Sitecore.JavaScriptServices.Configuration;
    using Sitecore.JavaScriptServices.ViewEngine.Presentation;
    using Sitecore.JavaScriptServices.ViewEngine.RenderingEngine;
    using Sitecore.LayoutService.Configuration;
    using Sitecore.LayoutService.ItemRendering;
    using Sitecore.LayoutService.Serialization;
    using Sitecore.Mvc.Presentation;

    public class NuxtJsLayoutRenderer : JsLayoutRenderer
    {
        public NuxtJsLayoutRenderer(
            Rendering rendering,
            AppConfiguration appConfig,
            NamedConfiguration layoutServiceNamedConfig,
            ILayoutService layoutService,
            ISerializerService serializerService,
            IJssRendererConfiguration jssRendererConfiguration)
            : base(rendering, appConfig, layoutServiceNamedConfig, layoutService, serializerService, jssRendererConfiguration)
        {
        }

        protected override void PerformRender(
            TextWriter writer,
            IRenderEngine renderEngine,
            string moduleName,
            string functionName,
            object[] functionArgs)
        {
            // todo: improve parsing and error handling. should be wrapped in object eventually
            // currently all the error handling is done on Nuxt side but possibly worth adding processing of non 200 codes in NuxtRenderEngine class
            try
            {
                var customRenderEngine = (NuxtRenderEngine)renderEngine;
                var renderResult = customRenderEngine.InvokeAsString(moduleName, functionName, functionArgs);
                writer.Write(renderResult);
            }
            catch (Exception ex)
            {
                writer.Write(ex.Message);
            }
        }
    }
}
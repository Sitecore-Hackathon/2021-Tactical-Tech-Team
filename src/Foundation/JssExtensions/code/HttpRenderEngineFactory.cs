namespace TTT.Foundation.JssExtensions
{
    using Sitecore.Diagnostics;
    using Sitecore.JavaScriptServices.ViewEngine.Http;
    using Sitecore.JavaScriptServices.ViewEngine.RenderingEngine;

    public class HttpRenderEngineFactory : IHttpRenderEngineFactory, IRenderEngineFactory
    {
        protected readonly IRenderEngineOptionsResolver RenderEngineOptionsResolver;
        protected readonly IHttpClientFactory HttpClientFactory;

        public HttpRenderEngineFactory(
            IRenderEngineOptionsResolver renderEngineOptionsResolver,
            IHttpClientFactory httpClientFactory)
        {
            Assert.ArgumentNotNull(renderEngineOptionsResolver, nameof(renderEngineOptionsResolver));
            Assert.ArgumentNotNull(httpClientFactory, nameof(httpClientFactory));
            this.RenderEngineOptionsResolver = renderEngineOptionsResolver;
            this.HttpClientFactory = httpClientFactory;
        }

        public virtual IRenderEngine CreateEngine(RenderEngineOptions options)
        {
            Assert.ArgumentNotNull(options, nameof(options));
            return new NuxtRenderEngine(this.HttpClientFactory, this.RenderEngineOptionsResolver.ResolveForId(options.Id, options));
        }
    }
}
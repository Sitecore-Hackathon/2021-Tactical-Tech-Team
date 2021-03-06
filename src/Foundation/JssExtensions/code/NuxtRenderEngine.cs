namespace TTT.Foundation.JssExtensions
{
    using System;

    using Sitecore.Diagnostics;
    using Sitecore.JavaScriptServices.ViewEngine.Http;

    public class NuxtRenderEngine : RenderEngine
    {
        public NuxtRenderEngine(IHttpClientFactory httpClientFactory, HttpRenderEngineOptions options)
            : base(httpClientFactory, options)
        {
        }

        public virtual string InvokeAsString(
            string moduleName,
            string functionName,
            params object[] functionArgs)
        {
            using (IHttpClient httpClient = this.HttpClientFactory.Create(this.Options))
            {
                string data = string.Empty;

                try
                {
                    // todo: should have more status codes handling in future
                    data = this.GetPayloadJson(moduleName, functionName, functionArgs);
                    var response = httpClient.Post(this.Options.EndpointUrl, data);
                    if (this.Options.EnableRelativeLinkProcessing)
                    {
                        response = this.ProcessHtml(this.Options.ApplicationUrl, response);
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    Log.Error("[JSS] Error occurred during POST to remote rendering host: `" + this.Options.EndpointUrl + "`", this);
                    Log.Debug("[JSS] Payload JSON for POST to remote rendering host: `" + data + "`");
                    Log.Error(ex.Message, ex, this);
                    throw;
                }
            }
        }
    }
}
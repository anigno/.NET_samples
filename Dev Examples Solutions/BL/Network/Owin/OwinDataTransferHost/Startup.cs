#region

using System.Web.Http;
using Owin;

#endregion

namespace OwinDataTransferHost
{
    public class Startup
    {
        // This method is required by Katana:

        #region Public Methods

        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();
            // Use the extension method provided by the WebApi.Owin library:
            app.UseWebApi(webApiConfiguration);
        }

        #endregion

        #region Private Methods

        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional});
            return config;
        }

        #endregion
    }
}
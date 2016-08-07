using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace AsposeWeb.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "words/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // clear the supported mediatypes of the xml formatter
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();


            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}

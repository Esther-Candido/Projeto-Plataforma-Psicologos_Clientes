using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace apiAgenda
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuração e serviços de API Web

            // Forçar a API a responder com JSON quando solicitado text/html
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Rotas de API Web
            config.MapHttpAttributeRoutes();

            //api/Event/Create ...Delete...
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

using System.Web.Http;
using WebActivatorEx;
using SpaceNeedle.Apps.Auth.Api.Net4;
using Swashbuckle.Application;
using System.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace SpaceNeedle.Apps.Auth.Api.Net4
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Phone Api");

                    c.PrettyPrint();

                    c.IncludeXmlComments(GetXmlCommentsPath());

                    c.UseFullTypeNameInSchemaIds();

                    c.DescribeAllEnumsAsStrings();

                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("Phone Api");
                });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\WebApiSwagger.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PhoneApi.Utils
{
    public class JsonContent : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContent(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }

        public static void SetupNullIgnore(HttpConfiguration config)
        {
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    //Fix exclusivamente para o swagger
                    NullValueHandling = NullValueHandling.Ignore
                }
            };
            config.Services.Replace(typeof(IContentNegotiator), new JsonContent(jsonFormatter));

        }

        public static void Setup(HttpConfiguration config)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContent(jsonFormatter));
        }
    }
}
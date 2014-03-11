using Freebase;
using RestSharp;
using System.Net;
namespace FreebaseTests
    {

    public static class ConnectToFreebase
        {

        public static HttpStatusCode ConnectByDynamic(dynamic d)
        {
            var ParseToMql = new ParseToMql(d);

            var client = new RestClient { BaseUrl = "https://www.googleapis.com/freebase/v1/mqlread" };

            var request = new RestRequest(Method.GET);

            request.AddParameter("query", ParseToMql.JsonString);

            request.RequestFormat = DataFormat.Json;

            return client.Execute(request).StatusCode;
        }
        }
    }

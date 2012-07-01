using System;
using RestSharp;
using Freebase;
namespace FreebaseTests
    {

    static class ConnectToFreebase
        {

        public static IRestResponse Connect ( Question question )
            {
            var parsedString = new ParseToMql(question);

            var client = new RestClient { BaseUrl = "https://www.googleapis.com/freebase/v1/mqlread" };

            var request = new RestRequest(Method.GET);

            request.AddParameter("query",   parsedString.JsonString );

            request.RequestFormat = DataFormat.Json;

            Console.WriteLine(parsedString.JsonString);
            return client.Execute(request);
            }
        }
    }

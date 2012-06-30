using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Freebase;
using FreebaseTests;
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

            return client.Execute(request);
            }
        }
    }

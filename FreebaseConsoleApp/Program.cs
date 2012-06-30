using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Freebase;
namespace FreebaseConsoleApp
    {
    public class Question
        {
        public String type { get; set; }
        public String name { get; set; }
        public Dictionary<Object, Object> album { get; set; }
        public Question ()
            {
            album = new Dictionary<Object, Object>();
            }
        }
    class Program
        {
        static void Main ( string[] args )
            {
            var thepolice = new Question { type = "/music/artist", name = "The Police" };
                thepolice.album.Add("name", new Object[100]);

            var response = ConnectToFreebase(thepolice);

            Console.WriteLine(response.Content);

            Console.ReadLine();

            }
        static IRestResponse ConnectToFreebase ( Question question )
            {
            var parsedString = new ParseToMql(question);

            var client = new RestClient { BaseUrl = "https://www.googleapis.com/freebase/v1/mqlread" };

            var request = new RestRequest(Method.GET);

            request.AddParameter("query",   parsedString.JsonString );

            request.RequestFormat = DataFormat.Json;

            //Console.WriteLine(request.Parameters[0]);

            return client.Execute(request);
            }
        }
    }

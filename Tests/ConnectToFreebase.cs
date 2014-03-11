using Freebase;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Tests
{
    public static class ConnectToFreebase
    {
        public static Uri AttachParameters(this Uri uri, NameValueCollection parameters)
        {
            var stringBuilder = new StringBuilder();
            string str = "?";
            for (int index = 0; index < parameters.Count; ++index)
            {
                stringBuilder.Append(str + parameters.AllKeys[index] + "=" + parameters[index]);
                str = "&";
            }
            return new Uri(uri + stringBuilder.ToString());
        }
        public static HttpStatusCode ConnectByDynamic(dynamic d)
	        {
	            var parsed = new ParseToMql(d);
	            var client = new HttpClient();
                var uri = new Uri("https://www.googleapis.com/freebase/v1/mqlread").
                                    AttachParameters(new NameValueCollection{{"query",parsed.JsonString}});
                var responseMessage = client.GetAsync(uri).Result;
	            return responseMessage.StatusCode;
	        }

    }
}

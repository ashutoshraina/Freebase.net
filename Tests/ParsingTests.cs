using System;
using System.Collections.Generic;
using System.Dynamic;
using Xunit;

namespace Tests
{
    public class ParsingTests
    {
    public class ParsingTest
        {
        [Fact]
        public void ParseTest ()
            {
                dynamic thepolice = new ExpandoObject();
                thepolice.type = "/music/artist";
                thepolice.album = new Object[31];
                thepolice.name = "The Police";          
                var statuscode = ConnectToFreebase.ConnectByDynamic(thepolice);
                Assert.Equal(System.Net.HttpStatusCode.OK, statuscode);
            }

        [Fact]
        public void ParseTestForDictionary()
            {
                dynamic thepolice = new ExpandoObject();
                thepolice.type = "/music/artist";
                thepolice.name = "The Police";
                thepolice.album = new Dictionary<Object, Object>();
                thepolice.album.Add("name", null);
                thepolice.album.Add("limit", 2);
                thepolice.album.Add("genre", new Object[5]);
                var statuscode = ConnectToFreebase.ConnectByDynamic(thepolice);
                Assert.Equal(System.Net.HttpStatusCode.OK, statuscode);
            }

        [Fact]
        public void ParseTestForDynamic()
        {
            dynamic thepolice = new ExpandoObject();
            thepolice.type = "/music/artist";
            thepolice.name = "The Police";
            thepolice.album = new Dictionary<Object, Object>();
            thepolice.album.Add("name", null);
            thepolice.album.Add("limit", 2);
            thepolice.album.Add("genre", new Object[5]);
            var statuscode = ConnectToFreebase.ConnectByDynamic(thepolice);
            Assert.Equal(System.Net.HttpStatusCode.OK, statuscode);
        }

        [Fact]
        public void ParseTestForMultipleDictionaries()
        {
            dynamic d = new ExpandoObject();
            d.name = null;
            d.id = null;
            d.type = "/film/director";
            ((IDictionary<String, Object>)d).Add("a:film", new Dictionary<Object, Object>
                                                           {
                                                                {"name",null}, {"id",null},
                                                                {"starring",new Dictionary<Object,Object>{ {"actor","Tobey Maguire"}}
                                                                }    
                                                           });
            var statuscode = ConnectToFreebase.ConnectByDynamic(d);
            Assert.Equal(System.Net.HttpStatusCode.OK, statuscode);
        }
    }
  }
}
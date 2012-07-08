using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FreebaseTests
    {
    [TestClass]
    public class ParsingTest
        {
        [TestMethod]
        public void ParseTest ()
            {
                dynamic thepolice = new ExpandoObject();
                thepolice.type = "/music/artist";
                thepolice.album = new Object[31];
                thepolice.name = "The Police";          
                var statuscode = ConnectToFreebase.ConnectByDynamic(thepolice);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, statuscode);
            }
        [TestMethod]
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
                Assert.AreEqual(System.Net.HttpStatusCode.OK, statuscode);
            }
        [TestMethod]
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
            Assert.AreEqual(System.Net.HttpStatusCode.OK, statuscode);
        }

        [TestMethod]
        public void ParseTestForMultipleDictionaries()
        {
            dynamic d = new ExpandoObject();
            d.name = null;
            d.id = null;
            d.type = "/film/director";
            ((IDictionary<String, Object>)d).Add("a:film", new Dictionary<Object, Object>() {
                                                                                               {"name",null},
                                                                                               {"id",null},
                                                                                               {"starring",new Dictionary<Object,Object>()
                                                                                               {
                                                                                                   {"actor","Tobey Maguire"}
                                                                                               }
                                                                                               }    
                                                                                            });
            var statuscode = ConnectToFreebase.ConnectByDynamic(d);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, statuscode);
        }

        }
  }
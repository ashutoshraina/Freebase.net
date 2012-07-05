using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
namespace FreebaseTests
    {
    [TestClass]
    public class ParsingTest
        {
        [TestMethod]
        public void ParseTest ()
            {
                var thepolice = new Question { type = "/music/artist", name = "The Police" ,album = null};
                var response = ConnectToFreebase.Connect(thepolice);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        [TestMethod]
        public void ParseTestForDictionary()
        {
            var thepolice = new Question { type = "/music/artist", name = "The Police" };
            thepolice.album.Add("name", null);
            thepolice.album.Add("limit", 2);
            thepolice.album.Add("genre", new Object[5]);
            var response = ConnectToFreebase.Connect(thepolice);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
        }
    }
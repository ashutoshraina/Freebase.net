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
            var thepolice = new Question { type = "/music/artist", name = "The Police" };
            thepolice.album.Add("name", new Object[100]);
            //dynamic d = new ExpandoObject();
            //d.type = "/music/artist";
            //d.name = "The Police";
            //d.album = new List<String>();
            var response = ConnectToFreebase.Connect(thepolice);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            }
        }
    }
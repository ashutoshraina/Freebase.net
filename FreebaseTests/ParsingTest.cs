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
            //   thepolice.album = new Tuple<string, IEnumerable<string>>("name",new List<String>());
            var response = ConnectToFreebase.Connect(thepolice);
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

            }
        }
    }
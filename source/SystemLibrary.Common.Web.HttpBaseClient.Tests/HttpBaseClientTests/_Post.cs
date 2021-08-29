﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemLibrary.Common.Web.HttpBaseClientTests
{
    partial class BaseHttpClientTests
    {
        [TestMethod]
        public void Post_PlainText_Success()
        {
            var webService = new HttpBinClient();
            
            var response = webService.Post("hello world", MediaType.textplain);

            Assert.IsTrue(response.Data.Contains("hello world"));
        }

        [TestMethod]
        public void Post_UrlEncoded_Success()
        {
            var webService = new HttpBinClient();

            var response = webService.PostUrlEncoded("hello=world&hello2=world2");

            Assert.IsTrue(response.Data.Contains("hello=world&hello2=world2"));
        }


        [TestMethod]
        public void Post_Json_Success()
        {
            var webService = new HttpBinClient();

            var response = webService.Post("{ hello:\"world\" }", MediaType.json);

            System.IO.File.AppendAllText(@"C:\Logs\text.txt", response.Data);
            Assert.IsTrue(response.Data.Contains("world"));
        }
        
        [TestMethod]
        public void Post_Poco_As_Json_Success()
        {
            var webService = new HttpBinClient();

            var car = new Car();
            car.Name = "world";

            var response = webService.Post(car, MediaType.json);

            Assert.IsTrue(response.Data.Contains("world"));
        }
    }
}

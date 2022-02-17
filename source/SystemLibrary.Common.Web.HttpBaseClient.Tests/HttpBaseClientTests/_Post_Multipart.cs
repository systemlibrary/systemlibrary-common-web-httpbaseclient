using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemLibrary.Common.Net;
using SystemLibrary.Common.Net.Json;

namespace SystemLibrary.Common.Web.HttpBaseClientTests
{
    partial class HttpBaseClientTests
    {
        class Form
        {
            public string file { get; set; }
        }

        [TestMethod]
        public void Post_Multipart_Success()
        {
            var bytes = Assemblies.GetEmbeddedResourceAsBytes("HttpBaseClientTests/Files", "text.json");

            var webService = new HttpBinClient();

            var response = webService.Post(bytes, MediaType.multipartFormData);

            var form = response.Data.PartialJson<Form>();

            Assert.IsTrue(form.file != null && form.file.Length > 100);
        }
    }
}

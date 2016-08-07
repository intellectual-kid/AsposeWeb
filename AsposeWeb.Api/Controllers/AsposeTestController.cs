using Aspose.Words;
using AsposeWeb.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AsposeWeb.Api.Controllers
{
    public class AsposeTestController : ApiController
    {
        
        [Route("words/{fileName}/protection")]        
        public IHttpActionResult Get(string fileName)
        {
            try
            {
                var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");

                if (File.Exists(uploadPath + "\\" + fileName))
                {

                    Document doc = new Document(uploadPath + "\\" + fileName);

                    var returnData = new ProtectionData()
                    {
                        ProtectionType = doc.ProtectionType.ToString()
                    };
                    return Ok<ProtectionData>(returnData);

                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [Route("words/{fileName}/protection")]
        public IHttpActionResult Post(string fileName, [FromBody]Protection protect)
        {
            try
            {

                if (protect == null)
                {
                    return BadRequest();
                }

                var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
                Document doc = new Document(uploadPath + "\\" + fileName);
                switch (protect.ProtectionType.ToLower())
                {
                    case "readonly":
                        doc.Protect(ProtectionType.ReadOnly, protect.NewPassword);
                        break;
                    case "allowonlycomments":
                        doc.Protect(ProtectionType.AllowOnlyComments, protect.NewPassword);
                        break;
                    case "allowonlyformfields":
                        doc.Protect(ProtectionType.AllowOnlyFormFields, protect.NewPassword);
                        break;
                    case "allowonlyrevisions":
                        doc.Protect(ProtectionType.AllowOnlyRevisions, protect.NewPassword);
                        break;
                    default:
                        doc.Protect(ProtectionType.NoProtection, protect.NewPassword);
                        break;

                }
                var returnData = new ProtectionData()
                {
                    ProtectionType = doc.ProtectionType.ToString()
                };
                return Ok<ProtectionData>(returnData);


            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        
        [MimeMultipart]
        [Route("words/convert")]
        public async Task<FileUploadResult> Put()
        {
            try
            {
                string fileName = string.Empty;
                var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");

                var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

                // Read the MIME multipart asynchronously 
                await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

                string _localFileName = multipartFormDataStreamProvider
                    .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();


                Document doc = new Document(_localFileName);

                fileName = uploadPath + "\\test.pdf";

                // Save the document in PDF format. 
                doc.Save(fileName);


                // Create response
                return new FileUploadResult
                {
                    LocalFilePath = fileName,

                    FileName = Path.GetFileName(fileName),

                    FileLength = new FileInfo(fileName).Length
                };
            }
            catch (Exception)
            {
                return new FileUploadResult();
            }
            
        }

    }
}

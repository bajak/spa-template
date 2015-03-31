using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace spa.web
{
    public class ControllerHelper
    {
        private static int _fileId;


        public async static Task<dynamic> CopyMulipartContent(ApiController controller)
        {
            if (!controller.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            var tempPath = HttpContext.Current.Server.MapPath("~/Content/Temp/");
            var fileNames = new List<string>();

            await controller.Request.Content.ReadAsMultipartAsync(provider);

            foreach (MultipartFileData file in provider.FileData)
            {
                _fileId++;
                if (_fileId + 1 > Int32.MaxValue)
                    _fileId = 0;
                var filename = tempPath + _fileId + "_" + file.Headers.ContentDisposition.FileName.Replace("\"", "").Replace("\\", "");
                fileNames.Add(filename);
                File.Copy(file.LocalFileName, filename);
                FileHelper.WaitFileUnlockedAsync(() => File.Delete(file.LocalFileName), file.LocalFileName, 30, 800);
            }

            return new { formData = provider.FormData, fileNames };
        }
    }
}
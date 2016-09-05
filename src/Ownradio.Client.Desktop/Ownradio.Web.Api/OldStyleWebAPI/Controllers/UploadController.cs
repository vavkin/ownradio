using OldStyleWebAPI.Infrastructure;
using OldStyleWebAPI.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OldStyleWebAPI.Controllers
{
    public class UploadController : ApiController
    {
        public string Get()
        {
            return "GET!!!";
        }

        // Изменение атрибутов загруженного файла
        [Route("api/upload/{guid},{fileName},{path},{userid}")]
        public string Get(string guid, string filename, string path, string userid)
        {
            var musicFile = new MusicFile()
            {
                id = Guid.Parse(guid),
                fileName = filename.Replace('_', '.'),
                path = path.Replace('|', '\\'),
                userId = Guid.Parse(userid)
            };
            musicFile.Save();
            return "OK";
        }

        // Получение файла
        [MimeMultipart]
        public async Task<FileUploadResult> Post()
        {
            var path = "~/App_Data/uploads";
            if (Request.Headers.Contains("userId"))
            {
                path += "/" + Request.Headers.GetValues("userId").ToArray<string>()[0];
                
            }
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path); // почему-то создает по-нарошку :-(

            // Получаем путь
            var uploadPath = HttpContext.Current.Server.MapPath(path);

            // Создаем провайдер загрузки
            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Асинхронно читаем данные
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            // Получаем имя файла
            string localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

            // Create response
            return new FileUploadResult
            {
                LocalFilePath = localFileName,
                FileName = Path.GetFileName(localFileName),
                FileLength = new FileInfo(localFileName).Length
            };
        }
    }
}
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
			musicFile.registerFile();
			return "OK";
		}

		// Получение и сохранение файла
		[MimeMultipart]
		public async Task<FileUploadResult> Post()
		{
			try
			{
				var path = "~/App_Data/uploads";
				if (Request.Headers.Contains("userId"))
				{
					path += "/" + Request.Headers.GetValues("userId").ToArray<string>()[0];

				}
				
				// Получаем путь
				var uploadPath = HttpContext.Current.Server.MapPath(path);

				// Если директория не существует, то создает ее
				if (!Directory.Exists(uploadPath))
					Directory.CreateDirectory(uploadPath);

				// Создаем провайдер загрузки
				var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

				// Асинхронно читаем данные
				await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

				var foo = multipartFormDataStreamProvider.FormData;

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
			catch(Exception ex)
			{
				var str = ex.Message;
			}
			return null;
		}
	}
}
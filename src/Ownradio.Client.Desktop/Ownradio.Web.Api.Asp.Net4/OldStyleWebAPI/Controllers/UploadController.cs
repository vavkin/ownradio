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
		// Получение и сохранение файла
		[MimeMultipart]
		public async Task<FileUploadResult> Post()
		{
			try
			{
				// Получаем из заголовков идентификатор пользователя
				var userId = Request.Headers.GetValues("userId").ToArray<string>()[0];
				// Формируем относительный путь для загрузки файлов пользователя
				var path = string.Format("~/App_Data/{0}", userId);
				// Получаем полный путь для загрузки файлов пользователя
				var uploadPath = HttpContext.Current.Server.MapPath(path);
				// Если директория не существует, то создает ее
				if (!Directory.Exists(uploadPath))
					Directory.CreateDirectory(uploadPath);

				// Создаем провайдер загрузки
				var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);
				// Асинхронно читаем данные
				await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);
				// Получаем имя файла
				string localFileName = multipartFormDataStreamProvider
					.FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
				// Создаем ответ клиенту
				var result = new FileUploadResult
				{
					LocalFilePath = localFileName,
					FileName = Path.GetFileName(localFileName),
					FileLength = new FileInfo(localFileName).Length
				};

				// Получаем данные о загружаемом файле
				var tags = multipartFormDataStreamProvider.FormData;
				// Формируем объект класса файл из полученных данных о файле
				var musicFile = new MusicFile()
				{
					id = Guid.Parse(tags["fileGuid"]),
					fileName = tags["filename"],
					path = tags["filePath"],
					userId = Guid.Parse(userId)
				};
				// Добавляем в БД информацию о принятом файле
				musicFile.registerFile();
				// Возвращаем результат загрузки клиенту
				return result;
			}
			catch(Exception ex)
			{
				var str = ex.Message;
			}
			return null;
		}
	}
}
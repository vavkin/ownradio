using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OwnRadio.Web.Api.Infrastructure;
using System.IO;
using Microsoft.AspNetCore.Http;
using OwnRadio.Web.Api.Models;

namespace OwnRadio.Web.Api.Controllers
{
	[Route("api/[controller]")]
	public class UploadController : Controller
	{
		// POST api/upload
		[MimeMultipart]
		// Получение и сохранение файла
		public async Task<FileUploadResult> Post(IFormFile musicFile, string fileGuid, string fileName, string filePath, string userId)
		{
			FileUploadResult result = null;
			try
			{
				// Получаем строку соединения из конфигурационного файла
				// TODO: доставать из настроек
				var connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=1;Database=ownRadio;";

				// Получаем папку для загрузки файлов пользователя
				// TODO: доставать из настроек
				var uploadFolder = "c:\\Uploads"; 

				// Формируем путь для загрузки файлов пользователя
				var uploadPath = string.Format("{0}\\{1}", uploadFolder, userId);
				// Если директория не существует, то создает ее
				if (!Directory.Exists(uploadPath))
					Directory.CreateDirectory(uploadPath);

				// Если длина файла не нулевая, то начинаем получение файла
				if (musicFile.Length > 0)
				{
					var fullFileName = Path.Combine(uploadPath, musicFile.FileName);
					using (var fileStream = new FileStream(fullFileName, FileMode.Create))
					{
						await musicFile.CopyToAsync(fileStream);
					}
					// Создаем ответ клиенту
					result = new FileUploadResult
					{
						LocalFilePath = musicFile.FileName,
						FileName = Path.GetFileName(fullFileName),
						FileLength = new FileInfo(fullFileName).Length
					};
					// Формируем объект класса файл из полученных данных о файле
					var newMusicFile = new MusicFile(connectionString)
					{
						id = Guid.Parse(fileGuid),
						fileName = fileName,
						path = filePath,
						userId = Guid.Parse(userId)
					};
					// Добавляем в БД информацию о принятом файле
					newMusicFile.registerFile();
				}

			}
			catch (Exception ex)
			{
				var message = ex.Message;
			}
			return result;
		}
	}
}

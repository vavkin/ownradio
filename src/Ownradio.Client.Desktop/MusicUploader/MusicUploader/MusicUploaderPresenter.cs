using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwnRadio.DesktopPlayer
{
	class MusicUploaderPresenter
	{
		// Слой доступа к данным
		private DataAccessLayer dal;
		// Настройки программы
		public Settings settings;
		// Очередь загрузки
		public List<MusicFile> uploadQueue;
		private Logger log;

		public MusicUploaderPresenter(Logger logger)
		{
			log = logger;
			try
			{
				// Получаем настройки
				settings = new Settings(log);
				// Слой доступа к данным
				dal = new DataAccessLayer(settings, log);
				// загружаем сохраненную очередь
				uploadQueue = dal.getNotUploaded();
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Получает список файлов из папки и добавляет их в очередь загрузки
		public void getQueue(string path)
		{
			try
			{
				if (!string.IsNullOrEmpty(path))
				{
					// получаем список файлов
					List<string> filenames = new List<string>();
					getMusicFiles(path, ref filenames);
					// заполняем ListView файлами
					foreach (var file in filenames)
					{
						// Создаем объект - файл
						var musicFile = new MusicFile();
						// Получаем имя файла
						musicFile.fileName = Path.GetFileName(file);
						// Получаем относительный путь
						musicFile.filePath = Path.GetDirectoryName(file);
						// Присваиваем файлу идентификатор
						musicFile.fileGuid = Guid.NewGuid();
						if (dal.addToQueue(musicFile) > 0)
						{
							// Добавляем файл в очередь
							uploadQueue.Add(musicFile);
						}
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Получает список файлов из дирректории рекурсивно
		private void getMusicFiles(string sourceDirectory, ref List<string> filenames)
		{
			try
			{
				var allFiles = Directory.EnumerateFiles(sourceDirectory);
				// Оставляем только mp3
				var musicFiles = allFiles.Where(s => s.Split('.')[s.Split('.').Count() - 1].ToLower().Equals("mp3"));
				// добавляем все mp3 файлы в список
				filenames.AddRange(musicFiles);

				// получаем список папок в текущей папке
				var dirs = Directory.EnumerateDirectories(sourceDirectory);
				// рекурсивно получаем список файлов и проходим вложенные папки
				foreach (var directory in dirs)
					getMusicFiles(directory, ref filenames);
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Загружает файлы на сервер асинхронно
		public async void uploadMusicFilesAsync(MainForm.afterUploadActions afterUploadActions)
		{
			try
			{
				//Создаем новый объект потока для функции загрузки файлов
				var uploaded = await Task.Factory.StartNew(() => uploadFiles());
				// Удаляем из списка загруженное
				foreach (var item in uploaded)
				{
					uploadQueue.Remove(item);
				}
				// Выполняем действия на форме по завершении загрузки
				afterUploadActions();
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Загрузка музыкальных файлов на сервер
		private List<MusicFile> uploadFiles()
		{
			var uploaded = new List<MusicFile>();
			try
			{
				foreach (var musicFile in uploadQueue)
				{
					// Формируем полный путь к файлу
					var fullFileName = musicFile.filePath + "\\" + musicFile.fileName;
					// Открываем файловый поток
					var fileStream = File.Open(fullFileName, FileMode.Open);
					// Получаем информацию о файле
					var fileInfo = new FileInfo(fullFileName);
					FileUploadResult uploadResult = null;
					bool fileUploaded = false;
					// создаем контент
					var content = new MultipartFormDataContent();
					// добавляем в контент файловый поток
					content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", musicFile.fileGuid)// fileInfo.Name)
					);
					content.Headers.Add("userId", settings.userId);
					// Создаем http клиент
					HttpClient httpClient = new HttpClient();
					// Делаем асинхронный POST запрос для передачи файла
					Task taskUpload = httpClient.PostAsync(settings.serverAddress + "api/upload", content).ContinueWith(task =>
					{
						if (task.Status == TaskStatus.RanToCompletion)
						{
							var response = task.Result;
							if (response.IsSuccessStatusCode)
							{
								uploadResult = response.Content.ReadAsAsync<FileUploadResult>().Result;
								if (uploadResult != null)
									fileUploaded = true;
							}
						}

						fileStream.Dispose();
					});
					// ждем завершения загрузки
					taskUpload.Wait();
					if (fileUploaded)
					{
						// добавляем в БД на сервере информацию о загруженном файле
						var b = updateFileInfo(musicFile.fileGuid.ToString(), musicFile.fileName, musicFile.filePath);
						if (b.Result)
						{
							dal.markAsUploaded(musicFile);
							uploaded.Add(musicFile);
						}
					}
					httpClient.Dispose();
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
				MessageBox.Show(ex.Message);
			}
			return uploaded;
		}

		// Добавляет в БД на сервере информацию о файле
		private async Task<bool> updateFileInfo(string id, string fileName, string path)
		{
			try
			{
				// Создаем http клиент
				var client = new HttpClient();
				// Задаем адрес сервера
				client.BaseAddress = new Uri(settings.serverAddress);
				// В локальном пути подменяем символы "\" и "." для корректной работы GET запроса
				var localPath = path.Substring(3).Replace('\\', '|').Replace('.', '_');
				// В имени файла подменяем точку для корректной работы GET запроса
				var file = fileName.Replace('.', '_');
				// Выполняем асинхронный GET запрос
				var result = await client.GetAsync("api/upload/" + id + "," + file + "," + localPath + "," + settings.userId);
				client.Dispose();
				return result.StatusCode.Equals(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return false;
			}
		}
	}
}

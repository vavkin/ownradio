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
		private DataAccessLayer dal;					// Слой доступа к данным
		public Settings settings;						// Настройки программы
		public List<MusicFile> uploadQueue;				// Очередь загрузки

		public MusicUploaderPresenter()
		{
			// Получаем настройки
			settings = new Settings();
			// Слой доступа к данным
			dal = new DataAccessLayer(settings);
			// загружаем сохраненную очередь
			uploadQueue = dal.getNotUploaded();

		}

		// Получает список файлов из папки и добавляет их в очередь загрузки
		public void getQueue(string path)
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

		// Получает список файлов из дирректории рекурсивно
		private void getMusicFiles(string sourceDirectory, ref List<string> filenames)
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

		// Загружает файлы на сервер асинхронно
		public async void uploadMusicFilesAsync(MainForm.afterUploadActions afterUploadActions)
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

		// Загрузка музыкальных файлов на сервер
		private List<MusicFile> uploadFiles()
		{
			var uploaded = new List<MusicFile>();
			try
			{
				foreach (var musicFile in uploadQueue)
				{
					HttpClient httpClient = new HttpClient();
					var fullFileName = musicFile.filePath + "\\" + musicFile.fileName;
					var fileStream = File.Open(fullFileName, FileMode.Open);
					var fileInfo = new FileInfo(fullFileName);
					FileUploadResult uploadResult = null;
					bool fileUploaded = false;
					var content = new MultipartFormDataContent();
					content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", musicFile.fileGuid)// fileInfo.Name)
					);
					content.Headers.Add("userId", settings.userId);

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

					taskUpload.Wait();
					if (fileUploaded)
					{
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
				MessageBox.Show(ex.Message);
			}
			return uploaded;
		}

		// Обновить на сервере информацию о файле (добавить в БД)
		private async Task<bool> updateFileInfo(string id, string fileName, string path)
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri(settings.serverAddress);
			var localPath = path.Substring(3).Replace('\\', '|').Replace('.', '_');
			var file = fileName.Replace('.', '_');
			var result = await client.GetAsync("api/upload/" + id + "," + file + "," + localPath + "," + settings.userId);
			client.Dispose();
			return result.StatusCode.Equals(HttpStatusCode.OK);
		}
	}
}

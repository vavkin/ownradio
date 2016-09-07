using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using NLog;

namespace OwnRadio.DesktopPlayer
{
	class DataAccessLayer
	{
		// Строка соединения
		SQLiteConnection connection;
		// Логгер
		private Logger log;

		public DataAccessLayer(Settings settings, Logger logger)
		{
			try
			{
				log = logger;
				var databaseFileName = settings.connectionString.Split('=')[1];
				// Если файл БД существует, то устанавливаем соединение
				if (File.Exists(databaseFileName))
				{
					// Подключаемся к БД
					connection = new SQLiteConnection(settings.connectionString);
					log.Debug("Установлено подключение к существующей БД");
				}
				// Если файл БД не существует, то создаем
				else
				{
					// Создаем БД
					SQLiteConnection.CreateFile(databaseFileName);
					// Подключаемся к БД
					connection = new SQLiteConnection(settings.connectionString);
					// Создаем таблицу Files
					var command = new SQLiteCommand("CREATE TABLE \"Files\" ( `Id` TEXT NOT NULL, `FileName` TEXT NOT NULL, `SubPath` TEXT, `Uploaded` NUMERIC DEFAULT 0, PRIMARY KEY(`id`) );", connection);
					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
					log.Debug("Создан новый файл БД");
				}
			}
			catch(Exception ex)
			{
				log.Error(ex);
				MessageBox.Show(ex.Message, "Ошибка инициализации DAL");
			}			
		}
		#region Queue
		// Добавить файл в список незагруженных
		public int addToQueue(MusicFile musicFile)
		{
			int rowsAffected = 0;
			try
			{
				// Открываем соединение
				connection.Open();
				// Сохраняем запись
				if (!exist(musicFile.fileName)) // которая не была сохранена ранее
				{
					// Формируем строку запроса
					var commandSQL = string.Format("INSERT INTO Files (Id, FileName, SubPath) VALUES ('{0}', '{1}', '{2}')", musicFile.fileGuid, musicFile.fileName, musicFile.filePath);
					// Создаем команду
					SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
					// выполняем команду
					rowsAffected += cmd.ExecuteNonQuery();
				}
				// Закрываем соединение
				connection.Close();
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
			return rowsAffected;
		}

		// Проверяет имеется ли в локальной БД файл
		private bool exist(string fileName)
		{
			var count = 0;
			try
			{
				// Формируем строку запроса
				var commandSQL = string.Format("SELECT count(*) FROM Files WHERE FileName LIKE '{0}'", fileName);
				// Создаем команду
				SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
				// Получаем количество записей
				var result = cmd.ExecuteScalar();
				count = Convert.ToInt16(result);
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
			return count > 0;
		}

		// Получает не загруженные файлы
		internal List<MusicFile> getNotUploaded()
		{
			var files = new List<MusicFile>();
			try
			{
				// Открываем соединение
				connection.Open();
				// Формируем строку запроса
				var commandSQL = "SELECT * FROM Files";
				// Создаем команду
				SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
				// Получаем ридер
				var reader = cmd.ExecuteReader();
				// Читаем
				while (reader.Read())
				{
					var file = new MusicFile()
					{
						fileGuid = Guid.Parse((string)reader["Id"]),
						fileName = (string)reader["FileName"],
						filePath = (string)reader["SubPath"],
						uploaded = (decimal)reader["Uploaded"] > 0
					};
					files.Add(file);
				}
				// Закрываем соединение
				connection.Close();
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
			return files;
		}

		// Помечает в БД файлы как загруженные
		internal bool markAsUploaded(MusicFile musicFile)
		{
			int count = 0;
			try
			{
				// Открываем соединение
				connection.Open();
				// Формируем строку запроса
				string commandSQL = string.Format("UPDATE Files SET Uploaded=1 WHERE Id LIKE '{0}'", musicFile.fileGuid);
				// Создаем команду
				SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
				// Получаем количество записей
				var result = cmd.ExecuteScalar();
				// Закрываем соединение
				connection.Close();

				count = Convert.ToInt16(result);
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
			return count > 0;
		}
		#endregion
	}
}

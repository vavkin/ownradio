using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;

namespace OwnRadio.DesktopPlayer
{
	class DataAccessLayer
	{
		SQLiteConnection connection;
		public DataAccessLayer(Settings settings)
		{
			try
			{
				var databaseFileName = settings.connectionString.Split('=')[1];
				// Если файл БД существует, то устанавливаем соединение
				if (File.Exists(databaseFileName))
				{
					// Подключаемся к БД
					connection = new SQLiteConnection(settings.connectionString);
				}
				// Если файл БД не существует, то создаем
				else
				{
					// Создаем БД
					SQLiteConnection.CreateFile(databaseFileName);
					// Подключаемся к БД
					connection = new SQLiteConnection(settings.connectionString);
					// Создаем таблицу Files
					var command = new SQLiteCommand("CREATE TABLE \"Files\" ( `id` TEXT NOT NULL, `fileName` TEXT NOT NULL, `subPath` TEXT, `uploaded` NUMERIC DEFAULT 0, PRIMARY KEY(`id`) );", connection);
					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка инициализации DAL");
			}			
		}
		#region Queue
		// Добавить файл в список незагруженных
		public int addToQueue(MusicFile musicFile)
		{
			int rowsAffected = 0;
			// Открываем соединение
			connection.Open();
			// Сохраняем запись
			if (!exist(musicFile.fileName)) // которая не была сохранена ранее
			{
				// Формируем строку запроса
				var commandSQL = string.Format("INSERT INTO Files (id, fileName, subPath) VALUES ('{0}', '{1}', '{2}')", musicFile.fileGuid, musicFile.fileName, musicFile.filePath);
				// Создаем команду
				SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
				// выполняем команду
				rowsAffected += cmd.ExecuteNonQuery();
			}
			// Закрываем соединение
			connection.Close();
			return rowsAffected;

		}

		// Проверяет имеется ли в локальной БД файл
		private bool exist(string fileName)
		{
			// Формируем строку запроса
			var commandSQL = string.Format("SELECT count(*) FROM Files WHERE fileName LIKE '{0}'", fileName);
			// Создаем команду
			SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
			// Получаем количество записей
			var result = cmd.ExecuteScalar();
			int count = Convert.ToInt16(result);
			return count > 0;
		}

		// Получает не загруженные файлы
		internal List<MusicFile> getNotUploaded()
		{
			var files = new List<MusicFile>();
			// Открываем соединение
			connection.Open();
			// Формируем строку запроса
			var commandSQL = "SELECT * FROM Files WHERE uploaded=0";
			// Создаем команду
			SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
			// Получаем ридер
			var reader = cmd.ExecuteReader();
			// Читаем
			while (reader.Read())
			{
				var file = new MusicFile()
				{
					fileGuid = Guid.Parse((string)reader["id"]),
					fileName = (string)reader["fileName"],
					filePath = (string)reader["subPath"]
				};
				files.Add(file);
			}
			// Закрываем соединение
			connection.Close();
			return files;
		}

		// Помечает в БД файлы как загруженные
		internal bool markAsUploaded(MusicFile musicFile)
		{
			// Открываем соединение
			connection.Open();
			// Формируем строку запроса
			string commandSQL = string.Format("UPDATE Files SET uploaded=1 WHERE id LIKE '{0}'", musicFile.fileGuid);
			// Создаем команду
			SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
			// Получаем количество записей
			var result = cmd.ExecuteScalar();
			// Закрываем соединение
			connection.Close();

			int count = Convert.ToInt16(result);
			return count > 0;
		}
		#endregion
	}
}

using Npgsql;
using System;
using System.Configuration;
using System.Data;

namespace OldStyleWebAPI.Models
{
	public class MusicFile
	{
		public Guid id { get; set; }			// Идентификатор файла
		public string fileName { get; set; }	// Имя файла
		public string path { get; set; }		// Путь к файлу на устройстве пользователя
		public Guid userId { get; set; }		// Идентификатор пользователя
		private string connectionString;		// Строка подключения к БД

		// Конструктор - инициализация данных
		public MusicFile()
		{
			// Получаем строку соединения из конфигурационного файла
			connectionString = ConfigurationManager.ConnectionStrings["OwnradioWebApi"].ConnectionString;
		}

		// Сохранение в БД информации о файле
		internal void registerFile()
		{
			using (var npgSqlConnection = new NpgsqlConnection(connectionString))
			{
				// Создаем комманду - с регистром имени функции проблема: преобразует имя функции в нижний регистр
				var npgSqlCommand = new NpgsqlCommand("RegisterFile", npgSqlConnection);
				// Уточняем тип комманды - хранимая процедура
				npgSqlCommand.CommandType = CommandType.StoredProcedure;
				// Добавляем параметры
				npgSqlCommand.Parameters.AddWithValue("ID", id);
				npgSqlCommand.Parameters.AddWithValue("FileName", fileName);
				npgSqlCommand.Parameters.AddWithValue("Path", path);
				npgSqlCommand.Parameters.AddWithValue("UserID", userId);
				// Открываем соединение
				npgSqlConnection.Open();
				// Выполняем хранимую процедуру (функцию)
				npgSqlCommand.ExecuteNonQuery();
			}
		}
	}
}
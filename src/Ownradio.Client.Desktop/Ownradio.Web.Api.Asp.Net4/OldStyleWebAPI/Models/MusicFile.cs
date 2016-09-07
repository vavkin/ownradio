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
			try
			{
				using (var npgSqlConnection = new NpgsqlConnection(connectionString))
				{
					// Создаем комманду - с регистром имени функции проблема: не видит
					var npgSqlCommand = new NpgsqlCommand("registerfile", npgSqlConnection);
					// Уточняем тип комманды - хранимая процедура
					npgSqlCommand.CommandType = CommandType.StoredProcedure;
					// Добавляем параметры
					npgSqlCommand.Parameters.AddWithValue("id", id);
					npgSqlCommand.Parameters.AddWithValue("fileName", fileName);
					npgSqlCommand.Parameters.AddWithValue("path", path);
					npgSqlCommand.Parameters.AddWithValue("userId", userId);
					// Открываем соединение
					npgSqlConnection.Open();
					// Выполняем хранимую процедуру (функцию)
					npgSqlCommand.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				// для отладки
				var message = ex.Message;
			}
		}
	}
}
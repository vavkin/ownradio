using System;
using Npgsql;
using System.Data;

namespace Ownradio.Web.Api.Models
{
	public class MusicFile
	{
		public Guid id { get; set; }			// Идентификатор файла
		public string fileName { get; set; }	// Имя файла
		public string path { get; set; }		// Путь к файлу на устройстве пользователя
		public Guid userId { get; set; }		// Идентификатор пользователя
		private string connectionString;		// Строка подключения к БД

		// Конструктор - инициализация данных
		public MusicFile(string connectionString)
		{
			this.connectionString = connectionString;
		}

		// Сохранение в БД информации о файле
		internal void registerFile()
		{
			using (var npgSqlConnection = new NpgsqlConnection(connectionString))
			{
				// Создаем комманду - с регистром имени функции проблема: не видит
				var npgSqlCommand = new NpgsqlCommand();
				// Указываем имя хранимой процедуры (функции)
				npgSqlCommand.CommandText = "RegisterFile";
				// Указываем подключение
				npgSqlCommand.Connection = npgSqlConnection;
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

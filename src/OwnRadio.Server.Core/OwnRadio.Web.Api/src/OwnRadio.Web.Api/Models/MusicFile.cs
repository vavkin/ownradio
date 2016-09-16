using System;
using Npgsql;
using System.Data;

namespace OwnRadio.Web.Api.Models
{
	public class MusicFile
	{
		// Идентификатор файла
		public Guid id { get; set; }
		// Путь к файлу на устройстве пользователя
		public string localDevicePathUpload { get; set; }
		// Путь хранения файлана сервере
		public string path { get; set; }
		// Идентификатор пользователя		
		public Guid userId { get; set; }
		// Строка подключения к БД
		private string connectionString;

		// Конструктор - инициализация данных
		public MusicFile(string connectionString)
		{
			this.connectionString = connectionString;
		}

		// Сохранение в БД информации о файле
		internal void registerTrack()
		{
			using (var npgSqlConnection = new NpgsqlConnection(connectionString))
			{
				// Создаем комманду - с регистром имени функции проблема: не видит
				var npgSqlCommand = new NpgsqlCommand();
				// Указываем имя хранимой процедуры (функции)
				npgSqlCommand.CommandText = "registertrack";
				// Указываем подключение
				npgSqlCommand.Connection = npgSqlConnection;
				// Уточняем тип комманды - хранимая процедура
				npgSqlCommand.CommandType = CommandType.StoredProcedure;
				// Добавляем параметры
				npgSqlCommand.Parameters.AddWithValue("trackid", id);
				npgSqlCommand.Parameters.AddWithValue("localdevicepathupload", localDevicePathUpload);
				npgSqlCommand.Parameters.AddWithValue("path", path);
				npgSqlCommand.Parameters.AddWithValue("userid", userId);
				// Открываем соединение
				npgSqlConnection.Open();
				// Выполняем хранимую процедуру (функцию)
				npgSqlCommand.ExecuteNonQuery();
			}
		}
	}
}

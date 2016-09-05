using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace OwnRadio.DesktopPlayer
{
    class DataAccessLayer
    {
        SQLiteConnection connection;
        public DataAccessLayer()
        {
            // Подключаемся к БД
            connection = new SQLiteConnection("Data Source=data.db");            
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
                string commandSQL = "INSERT INTO files (id, FileName, SubPath) VALUES ('" + musicFile.fileGuid + "', '" + musicFile.fileName + "', '" + musicFile.filePath + "')";
                // Создаем команду
                SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
                // выполняем команду
                rowsAffected += cmd.ExecuteNonQuery();
            }
            // Закрываем соединение
            connection.Close();
            return rowsAffected;

        }

        // Имеется ли в локальной базе этот файл
        private bool exist(string fileName)
        {
            // Формируем строку запроса
            string commandSQL = "SELECT count(*) FROM files WHERE FileName LIKE '" + fileName + "'";
            // Создаем команду
            SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
            // Получаем количество записей
            var result = cmd.ExecuteScalar();
            int count = Convert.ToInt16(result);
            return count > 0;
        }

        // Получить не загруженные файлы
        internal List<MusicFile> getNotUploaded()
        {
            var files = new List<MusicFile>();
            // Открываем соединение
            connection.Open();
            // Формируем строку запроса
            string commandSQL = "SELECT * FROM files WHERE Uploaded=0";
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
                    fileName = (string)reader["FileName"],
                    filePath = (string)reader["SubPath"]
                };
                files.Add(file);
            }
            // Закрываем соединение
            connection.Close();
            return files;
        }

        internal bool markAsUploaded(MusicFile musicFile)
        {
            // Открываем соединение
            connection.Open();
            // Формируем строку запроса
            string commandSQL = "UPDATE files SET Uploaded=1 WHERE id LIKE '" + musicFile.fileGuid + "'";
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
        #region Settings
        // Загрузить настройки
        internal Settings loadSettings()
        {
            var settings = new Settings();

            // Открываем соединение
            connection.Open();
            // Формируем строку запроса
            string commandSQL = "SELECT Value FROM Settings WHERE Name='serverAddress'";
            // Создаем команду
            SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
            // Получаем данные
            var data = cmd.ExecuteScalar();
            // Читаем
            settings.serverAddress = (string)data;
            // Формируем строку запроса
            commandSQL = "SELECT Value FROM Settings WHERE Name='userId'";
            // Создаем команду
            cmd = new SQLiteCommand(commandSQL, connection);
            // Получаем данные
            data = cmd.ExecuteScalar();
            // Читаем
            settings.userId = (string)data;
            // Закрываем соединение
            connection.Close();

            return settings;
        }
        // Сохранить настройки
        internal void saveSettings(Settings settings)
        {
            // Открываем соединение
            connection.Open();
            // Формируем строку запроса
            string commandSQL = "UPDATE Settings SET Value='" + settings.serverAddress + "' WHERE Name='serverAddress';";
            commandSQL += "UPDATE Settings SET Value='" + settings.userId + "' WHERE Name='userId'";
            // Создаем команду
            SQLiteCommand cmd = new SQLiteCommand(commandSQL, connection);
            // Получаем ридер
            var data = cmd.ExecuteNonQuery();
            // Закрываем соединение
            connection.Close();
        }
        #endregion
    }
}

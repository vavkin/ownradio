using System.Configuration;

namespace OwnRadio.DesktopPlayer
{
	// Настройки программы
	class Settings
	{
		// Путь к файлу на сервере
		public string serverAddress { get; set; }
		// Имя файла
		public string userId { get; set; }
		// Строка подключения
		public string connectionString { get; set; }

		// Конструктор - чтение настроек из конфигурации
		public Settings()
		{
			userId = ConfigurationManager.AppSettings["userId"];
			serverAddress = ConfigurationManager.AppSettings["serverAddress"];
			connectionString = ConfigurationManager.ConnectionStrings["OwnradioDesktopClient"].ConnectionString;
		}

		// Сохранение настроек
		public void updateSettings()
		{
			var currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			currentConfig.AppSettings.Settings["userId"].Value = userId;
			currentConfig.AppSettings.Settings["serverAddress"].Value = serverAddress;
			currentConfig.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}
	}
}

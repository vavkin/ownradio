using System;

namespace OwnRadio.DesktopPlayer
{
	// Класс - информация о музыкальном файле
	class MusicFile
	{
		// Имя файла
		public string fileName { get; set; }
		// Путь к файлу
		public string filePath { get; set; }
		// Имя файла на сервере
		public Guid fileGuid { get; set; }
		// Флаг "Загружен на сервер"
		public bool uploaded { get; set; }
	}
}

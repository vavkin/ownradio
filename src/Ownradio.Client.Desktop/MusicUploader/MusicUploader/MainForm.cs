using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwnRadio.DesktopPlayer
{
	public partial class MainForm : Form
	{
		// Контроллер
		MusicUploaderPresenter formLogic;
		// Делегат для обратного вызова по окончании загрузки
		public delegate void afterUploadActions();  

		public MainForm()
		{
			InitializeComponent();
			// Инициализируем логику
			formLogic = new MusicUploaderPresenter();
			// загружаем список файлов из очереди в контрол на форме
			loadData();
			// Устанавливаем доступность кнопки загрузки на сервер
			toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
		}

		// Загружает список файлов из очереди в контрол списка на форме
		private void loadData()
		{
			listViewFiles.Items.Clear();
			foreach (var musicFile in formLogic.uploadQueue)
			{
				// Добавляем в список на форме
				var item = new ListViewItem(musicFile.fileGuid.ToString());
				var subItem = new ListViewItem.ListViewSubItem();
				subItem.Text = musicFile.fileName;
				item.SubItems.Add(subItem);
				subItem = new ListViewItem.ListViewSubItem();
				subItem.Text = musicFile.filePath;
				item.SubItems.Add(subItem);
				listViewFiles.Items.Add(item);
			}
		}

		// Обработчик нажатия кнопки выбора папки
		private void toolStripButtonSelect_Click(object sender, EventArgs e)
		{
			// Открываем диалог 
			folderBrowserDialogMain.ShowDialog();
			// Заполняем очередь
			formLogic.getQueue(folderBrowserDialogMain.SelectedPath);
			// загружаем список файлов из очереди в контрол на форме
			loadData();
			// Если появились файлы в списке - открываем кнопку "загрузить"
			toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
		}

		// Запуск загрузки файлов
		private void toolStripButtonUpload_Click(object sender, EventArgs e)
		{
			// Запрещаем нажимать кнопку запуска чтобы не запустили дважды
			toolStripButtonUpload.Enabled = false;
			// Асинхронно загружаем файлы на сервер
			formLogic.uploadMusicFilesAsync(new afterUploadActions(afterUpload));
		}

		// Действия по завершении загрузки
		public void afterUpload()
		{
			// Обновляем список файлов в форме
			loadData();
			// Возобновляем возможность нажимать кнопку загрузки, если есть что загружать.
			toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
			MessageBox.Show("Загрузка завершена!", "Операция выполнена");
		}

		// Вызов формы настроек
		private void toolStripButtonSettings_Click(object sender, EventArgs e)
		{
			var settingsForm = new SettingsForm();
			settingsForm.settings = formLogic.settings;
			settingsForm.ShowDialog();
		}
	}
}

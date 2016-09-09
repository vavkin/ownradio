using NLog;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwnRadio.DesktopPlayer
{
	public partial class MainForm : Form
	{
		// Контроллер
		private MusicUploaderPresenter formLogic;
		// Логгер
		private Logger log;
		// Плеер
		private Player player;

		public MainForm()
		{
			InitializeComponent();
			// Создаем логгер
			log = LogManager.GetCurrentClassLogger();
			// Инициализируем логику
			formLogic = new MusicUploaderPresenter(log);
			// загружаем список файлов из очереди в контрол на форме
			loadData();
			// Устанавливаем доступность кнопки загрузки на сервер
			toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);

			player = new Player(log);
		}

		// Загружает список файлов из очереди в контрол списка на форме
		private void loadData()
		{
			try
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
					// Загруженные на сервер файлы помечаем зеленым
					if (musicFile.uploaded)
					{
						item.ForeColor = Color.Green;
					}
					listViewFiles.Items.Add(item);
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Обработчик нажатия кнопки выбора папки
		private void toolStripButtonSelect_Click(object sender, EventArgs e)
		{
			try
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
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Запуск загрузки файлов
		private async void toolStripButtonUpload_Click(object sender, EventArgs e)
		{
			try
			{
				// Запрещаем нажимать кнопку запуска чтобы не запустили дважды
				toolStripButtonUpload.Enabled = false;
				var progress = new Progress<string>(message => textBoxLog.Text += (message + Environment.NewLine));
				updateControls("Загрузка начата");
				// Асинхронно загружаем файлы на сервер
				await Task.Factory.StartNew(() => formLogic.uploadFiles(progress));
				updateControls("Загрузка завершена");
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Действия по завершении загрузки
		public void updateControls(string message)
		{
			try
			{
				// Обновляем список файлов в форме
				loadData();
				// Возобновляем возможность нажимать кнопку загрузки, если есть что загружать.
				toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
				textBoxLog.Text += message + Environment.NewLine;
				//MessageBox.Show("Загрузка завершена!", "Операция выполнена");
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		// Вызов формы настроек
		private void toolStripButtonSettings_Click(object sender, EventArgs e)
		{
			try
			{
				var settingsForm = new SettingsForm();
				settingsForm.settings = formLogic.settings;
				settingsForm.ShowDialog();
				player = new Player(log);
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			if (player.IsPause)
			{
				player.Play();
				toolStripButtonPlay.Text = " ⏸ ";
			}
			else
			{
				player.Pause();
				toolStripButtonPlay.Text = " ▶️ ";
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			player.Next();
		}
	}
}

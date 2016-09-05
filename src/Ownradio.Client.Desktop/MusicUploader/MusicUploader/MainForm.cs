using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwnRadio.DesktopPlayer
{
    public partial class MainForm : Form
    {
        DataAccessLayer dal;            // Слой доступа к данным
        Settings settings;              // Настройки программы
        List<MusicFile> uploadQueue;    // Очередь загрузки

        public MainForm()
        {
            InitializeComponent();
            // Слой доступа к данным
            dal = new DataAccessLayer();
            // Получаем настройки
            settings = dal.loadSettings();
            // загружаем сохраненную очередь
            uploadQueue = dal.getNotUploaded();
            // загружаем список из очереди
            loadData();
            // Делаем кнопки недоступными
            toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
        }

        private void loadData()
        {
            listViewFiles.Items.Clear();
            foreach (var musicFile in uploadQueue)
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

        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            folderBrowserDialogMain.ShowDialog();
            if(!string.IsNullOrEmpty(folderBrowserDialogMain.SelectedPath))
            {
                // получаем список файлов
                List<string> filenames = new List<string>();
                getMusicFiles(folderBrowserDialogMain.SelectedPath, ref filenames);
                // заполняем ListView файлами
                foreach(var file in filenames)
                {
                    // Создаем объект - файл
                    var musicFile = new MusicFile();
                    // Получаем имя файла
                    musicFile.fileName = Path.GetFileName(file);
                    // Получаем относительный путь
                    musicFile.filePath = Path.GetDirectoryName(file);
                    // Присваиваем файлу идентификатор
                    musicFile.fileGuid = Guid.NewGuid();
                    if (dal.addToQueue(musicFile) > 0)
                    {
                        // Добавляем файл в очередь
                        uploadQueue.Add(musicFile);
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
            }
            // Если появились файлы в списке - открываем кнопку "загрузить"
            toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
        }

        private void getMusicFiles(string sourceDirectory, ref List<string> filenames)
        {
            var allFiles = Directory.EnumerateFiles(sourceDirectory);
            // Оставляем только mp3
            var musicFiles = allFiles.Where(s => s.Split('.')[s.Split('.').Count()-1].ToLower().Equals("mp3"));
            // добавляем все mp3 файлы в список
            filenames.AddRange(musicFiles);

            // получаем список папок в текущей папке
            var dirs = Directory.EnumerateDirectories(sourceDirectory);
            // рекурсивно получаем список файлов и проходим вложенные папки
            foreach (var directory in dirs)
                getMusicFiles(directory, ref filenames);
        }

        // Запуск загрузки файлов
        private async void toolStripButtonUpload_Click(object sender, EventArgs e)
        {
            // Запрещаем нажимать кнопку запуска чтобы не запустили дважды
            toolStripButtonUpload.Enabled = false;
            //Создаем новый объект потока для функции загрузки файлов
            var uploaded = await Task.Factory.StartNew(() => uploadFiles());
            // Удаляем из списка загруженное
            foreach (var item in uploaded)
            {
                uploadQueue.Remove(item);
            }
            // Обновляем список файлов в форме
            loadData();
            // Возобновляем возможность нажимать кнопку загрузки, если есть что загружать.
            toolStripButtonUpload.Enabled = (listViewFiles.Items.Count > 0);
            MessageBox.Show("Загрузка завершена!", "Операция выполнена");
        }

        // Загрузка файлов
        private List<MusicFile> uploadFiles()
        {
            var uploaded = new List<MusicFile>();
            try
            {
                foreach (var musicFile in uploadQueue)
                {
                    HttpClient httpClient = new HttpClient();
                    var fullFileName = musicFile.filePath + "\\" + musicFile.fileName;
                    var fileStream = File.Open(fullFileName, FileMode.Open);
                    var fileInfo = new FileInfo(fullFileName);
                    FileUploadResult uploadResult = null;
                    bool fileUploaded = false;
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", musicFile.fileGuid)// fileInfo.Name)
                    );
                    content.Headers.Add("userId", settings.userId);

                    Task taskUpload = httpClient.PostAsync(settings.serverAddress + "api/upload", content).ContinueWith(task =>
                    {
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                            var response = task.Result;
                            if (response.IsSuccessStatusCode)
                            {
                                uploadResult = response.Content.ReadAsAsync<FileUploadResult>().Result;
                                if (uploadResult != null)
                                    fileUploaded = true;
                            }
                        }

                        fileStream.Dispose();
                    });

                    taskUpload.Wait();
                    if (fileUploaded)
                    {
                        var b = updateFileInfo(musicFile.fileGuid.ToString(), musicFile.fileName, musicFile.filePath);
                        if (b.Result)
                        {
                            dal.markAsUploaded(musicFile);
                            uploaded.Add(musicFile);
                        }
                    }
                    httpClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return uploaded;
        }

        // Обновить на сервере информацию о файле (добавить в БД)
        private async Task<bool> updateFileInfo(string id, string fileName, string path)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(settings.serverAddress);
            var localPath = path.Substring(3).Replace('\\', '|').Replace('.','_');
            var file = fileName.Replace('.', '_');
            var result = await client.GetAsync("api/upload/" + id + "," + file + "," + localPath + "," + settings.userId);
            client.Dispose();
            return result.StatusCode.Equals(HttpStatusCode.OK);
        }

        // Вызов формы настроек
        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.settings = settings;
            settingsForm.ShowDialog();
            dal.saveSettings(settings);
        }
    }
}

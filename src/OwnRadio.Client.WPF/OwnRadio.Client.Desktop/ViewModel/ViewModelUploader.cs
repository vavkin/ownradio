using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using OwnRadio.Client.Desktop.ViewModel.Commands;

namespace OwnRadio.Client.Desktop.ViewModel
{
    public class ViewModelUploader : DependencyObject
    {
        public UploadCommand UploadCommand { get; set; }
        
        public bool IsUploading
        {
            get { return (bool)GetValue(IsUploadingProperty); }
            set { SetValue(IsUploadingProperty, value); }
        }

        public static readonly DependencyProperty IsUploadingProperty =
            DependencyProperty.Register("IsUploading", typeof(bool), typeof(ViewModelUploader), new PropertyMetadata(false));
        
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(ViewModelUploader), new PropertyMetadata(""));
        
        private DataAccessLayer dal;
        private List<MusicFile> uploadQueue;
        
        public ViewModelUploader()
        {
            this.UploadCommand = new UploadCommand(this);

            try
            {
                dal = new DataAccessLayer();
                uploadQueue = dal.GetNotUploaded();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void GetQueue(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return;

                var filenames = new List<string>();
                GetMusicFiles(path, ref filenames);

                foreach (var file in filenames)
                {
                    var musicFile = new MusicFile
                    {
                        fileName = Path.GetFileName(file),
                        filePath = Path.GetDirectoryName(file),
                        fileGuid = Guid.NewGuid()
                    };

                    if (dal.AddToQueue(musicFile) > 0)
                    {
                        uploadQueue.Add(musicFile);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        
        private void GetMusicFiles(string sourceDirectory, ref List<string> filenames)
        {
            try
            {
                var allFiles = Directory.EnumerateFiles(sourceDirectory);
                var musicFiles = allFiles.Where(s => s.Split('.')[s.Split('.').Count() - 1].ToLower().Equals("mp3"));
                filenames.AddRange(musicFiles);

                var dirs = Directory.EnumerateDirectories(sourceDirectory);

                foreach (var directory in dirs)
                    GetMusicFiles(directory, ref filenames);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public async void UploadFiles()
        {
            int queued = uploadQueue.Count(s => !s.uploaded);
            int uploaded = 0;
            Status = $"{uploaded}/{queued}";
            IsUploading = true;

            try
            {
                foreach (var musicFile in uploadQueue.Where(s => !s.uploaded))
                {
                    var fullFileName = musicFile.filePath + "\\" + musicFile.fileName;
                    
                    var fileStream = File.Open(fullFileName, FileMode.Open);
                    byte[] byteArray = new byte[fileStream.Length];
                    fileStream.Read(byteArray, 0, (int)fileStream.Length);
                    
                    HttpClient httpClient = new HttpClient();
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    
                    form.Headers.Add("userId", ConfigurationManager.AppSettings["UserId"]);
                    form.Add(new StringContent(musicFile.fileGuid.ToString()), "fileGuid");
                    form.Add(new StringContent(musicFile.fileName), "fileName");
                    form.Add(new StringContent(musicFile.filePath), "filePath");
                    form.Add(new StringContent(ConfigurationManager.AppSettings["UserId"]), "userId");
                    
                    form.Add(new ByteArrayContent(byteArray, 0, byteArray.Count()), "musicFile", musicFile.fileGuid + ".mp3");
                    
                    HttpResponseMessage response = await httpClient.PostAsync(@"http://ownradio.ru/api/upload", form);
                    
                    response.EnsureSuccessStatusCode();
                    httpClient.Dispose();
                    
                    dal.MarkAsUploaded(musicFile);
                    Status = $"{++uploaded}/{queued}";
                }
                
                System.Windows.MessageBox.Show($"Файлов загружено: {uploaded}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            IsUploading = false;
        }
        
        public void Upload()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                IsUploading = false;
                return;
            }
            
            GetQueue(dialog.SelectedPath);
            UploadFiles();
        }
    }
}

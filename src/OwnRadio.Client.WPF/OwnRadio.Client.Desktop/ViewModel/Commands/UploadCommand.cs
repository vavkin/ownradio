using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace OwnRadio.Client.Desktop.ViewModel.Commands
{
    public class UploadCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var dirs = Directory.GetFiles(dialog.SelectedPath, "*.mp3");

            MessageBox.Show("Selected: " + dirs.Length);

            foreach (var dir in dirs)
            {
                // Upload
            }
        }
    }
}

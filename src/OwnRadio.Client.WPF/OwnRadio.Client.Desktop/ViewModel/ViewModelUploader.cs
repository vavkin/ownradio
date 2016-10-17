using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OwnRadio.Client.Desktop.ViewModel.Commands;

namespace OwnRadio.Client.Desktop.ViewModel
{
    public class ViewModelUploader : DependencyObject
    {
        public UploadCommand UploadCommand { get; set; }
        
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(ViewModelUploader), new PropertyMetadata(0));
        
        public ViewModelUploader()
        {
            this.UploadCommand = new UploadCommand();
        }

        public void Upload()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OwnRadio.Client.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Currently loaded track
        /// </summary>
        private Track currentTrack = null;

        public MainWindow()
        {
            InitializeComponent();

            // Play track upon startup
            playNextTrack();
        }

        /// <summary>
        /// Set error status text colored by red
        /// </summary>
        /// <param name="text"></param>
        private void setErrorStatus(string text)
        {
            statusText.Foreground = Brushes.Red;
            statusText.Text = text;
        }

        /// <summary>
        /// Set normal status text
        /// </summary>
        /// <param name="text"></param>
        private void setNormalStatus(string text)
        {
            statusText.Foreground = Brushes.Black;
            statusText.Text = text;
        }

        /// <summary>
        /// Obtains next track info throught WebClient
        /// and plays it in MediaElement control
        /// </summary>
        private void playNextTrack()
        {
            try
            {
                currentTrack = App.WebClient.GetNextTrack(Properties.Settings.Default.DeviceId).Result;
                mediaElement.Source = currentTrack.Uri;
                mediaElement.Play();
                setNormalStatus("Now playing: " + currentTrack.Id);
            }
            catch(Exception exception)
            {
                setErrorStatus("Error: " + exception.Message);
            }
        }

        /// <summary>
        /// Event handler fires then track's media failed for some reasons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            setErrorStatus("Media failed: " + e.ErrorException.Message);
        }

        /// <summary>
        /// Event handler fired then media reached the end
        /// Here track statistics sended to web service
        /// and starts play next track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                currentTrack.ListenEnd = DateTime.Now;
                currentTrack.Status = Track.Statuses.Listened;
                App.WebClient.SendStatus(Properties.Settings.Default.DeviceId, currentTrack);
                playNextTrack();
            }
            catch (Exception exception)
            {
                setErrorStatus("Error: " + exception.Message);
            }
        }

        /// <summary>
        /// Event handler fires then button Resume/Play pressed
        /// Resumes paused track and changes button's handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayPause_Play(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
            btnPlayPause.Click -= btnPlayPause_Play;
            btnPlayPause.Click += btnPlayPause_Pause;
            btnPlayPause.Content = "Pause";
        }

        /// <summary>
        /// Event handler fires then button Resume/Play pressed
        /// Pausing currently playing track and changes button's handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlayPause_Pause(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
            btnPlayPause.Click -= btnPlayPause_Pause;
            btnPlayPause.Click += btnPlayPause_Play;
            btnPlayPause.Content = "Resume";
        }

        /// <summary>
        /// Event handler fired then button Next pressed
        /// Stops currently playing track
        /// Sends track statistics to web service
        /// Starts plaing next track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mediaElement.Stop();
                currentTrack.ListenEnd = DateTime.Now;
                currentTrack.Status = Track.Statuses.Skipped;
                App.WebClient.SendStatus(Properties.Settings.Default.DeviceId, currentTrack);
                playNextTrack();
            }
            catch(Exception exception)
            {
                setErrorStatus("Error: " + exception.Message);
            }
        }
    }
}

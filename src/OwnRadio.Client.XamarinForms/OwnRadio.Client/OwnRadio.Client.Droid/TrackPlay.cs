using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System.IO;
using Android.Widget;
using Android.Media;
using Plugin.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(OwnRadio.Client.Droid.TrackPlay))]
namespace OwnRadio.Client.Droid
{
    [Register("TrackPlay")]

    class TrackPlay : ITrackPlay
    {
        String GUID = "-1";//for the 1st request
        String DeviceID;// = "297f55b4-d42c-4e30-b9d7-a802e7b7eed9";
        String Method = "новых"; // сделать список имеющихся
        int ListedTillTheEnd = -1;

        MediaPlayer Player = new MediaPlayer();

        public void CurrentTrackPlay(bool PlayerExistFlag, String FileName)
        {
            if (!PlayerExistFlag)
            {

                try
                {
                    Player.Reset();
                    if (File.Exists(FileName) == false)
                    {
                    }
                    Player.SetDataSource(FileName);

                    Player.Prepared += (s, e) =>
                    {
                        Player.Start();
                    };
                    Player.Prepare();
                    Player.Completion += Player_Completion;

                    //данный блок для получения информации о треке
                    //MediaMetadataRetriever mMediaMetaDataRetriever = new MediaMetadataRetriever();
                    //mMediaMetaDataRetriever.SetDataSource(FileName);
                    //String titleName = mMediaMetaDataRetriever.ExtractMetadata(MetadataKey.Title);
                    //String artistName = mMediaMetaDataRetriever.ExtractMetadata(MetadataKey.Artist);
                    //int durationMS = Convert.ToInt32(mMediaMetaDataRetriever.ExtractMetadata(MetadataKey.Duration));
                    //int duration = durationMS / 1000;
                    //int hour = duration / 3600;
                    //int min = (duration - hour * 3600) / 60;
                    //int sec = duration - (hour * 3600 + min * 60);
                    //String trackLength;
                    //if (hour == 0)
                    //{
                    //    trackLength = Convert.ToString(min + ":" + sec);
                    //}
                    //else
                    //{
                    //    trackLength = Convert.ToString(hour + ":" + min + ":" + sec);
                    //}

                    //if (titleName == null) titleName = "Unknown";
                    //if (artistName == null) artistName = "Unknown";
                    //String TrackInfo = artistName + " - " + titleName + " " + trackLength + "\n";
                    //return TrackInfo;
                }
                catch (Exception ex)
                {
                    PlayerExistFlag = false;
                    throw;
                }
            }
            else if (Player.IsPlaying)
            {
                try
                {
                    Player.Pause();
                }
                catch (Exception ex) { throw; }
            }
            else if (!Player.IsPlaying)
            {
                try
                {
                    Player.Start();
                }
                catch (Exception ex) { throw; }
            }
        }

        public void NextTrackPlay()
        {
            //PlayerExistFlag = false;
            if (Player.IsPlaying)
                Player.Stop();
            ListedTillTheEnd = -1;

            //история прослушивания
            //try
            //{
            //    StatusTrack.SetStatusTrack(DeviceID, GUID, ListedTillTheEnd, DateTime.Now);
            //}
            //catch (Exception ex)
            //{
            //    status.Text += ex.Message + "\n";
            //}
        }

        private void Player_Completion(object sender, EventArgs e)
        {
            //PlayerExistFlag = false;
            ListedTillTheEnd = 1;
            //история прослушивания
            //try
            //{
            //    StatusTrack.SetStatusTrack(DeviceID, GUID, ListedTillTheEnd, DateTime.Now);
            //}
            //catch (Exception ex)
            //{
            //}
            //TrackPlay();
        }
    }


}

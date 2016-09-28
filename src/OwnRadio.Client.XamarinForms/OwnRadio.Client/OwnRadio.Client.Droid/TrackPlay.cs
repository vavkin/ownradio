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
        int ListedTillTheEnd = -1;
        bool PlayerExistFlag = false;
        MediaPlayer Player = new MediaPlayer();
        String Method = "новых";
        String DeviceID;
        String FileName;
        ISetStatusTrack StatusTrack = new StatusTrack();

        public void CurrentTrackPlay()
        {
            DeviceID = CrossSettings.Current.GetValueOrDefault<Guid>("DeviceID", Guid.NewGuid()).ToString();

            if (!PlayerExistFlag)
            {
                IGetNextTrackID nextTrack = new NextTrackID();

                try
                {
                    GUID = nextTrack.GetNextTrackID(DeviceID, out Method);
                    if (File.Exists(FileName))
                    {
                        File.Delete(FileName);
                    }
                    IGetTrack track = new Track();
                    FileName = track.GetTrackByID(GUID);
                    PlayerExistFlag = true;
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
            PlayerExistFlag = false;
            if (Player.IsPlaying)
                Player.Stop();
            ListedTillTheEnd = -1;
            CurrentTrackPlay();
            //история прослушивания
            try
            {
                StatusTrack.SetStatusTrack(DeviceID, GUID, ListedTillTheEnd, DateTime.Now);
            }
            catch (Exception ex)
            {
            }
        }

        private void Player_Completion(object sender, EventArgs e)
        {
            //Высвобождение ресурсов текущего MediaPlayer'а и создание нового для корректной работы события Completion
            Player.Release();
            Player = new MediaPlayer();
            PlayerExistFlag = false;
            ListedTillTheEnd = 1;
            CurrentTrackPlay();
            //история прослушивания
            try
            {
                StatusTrack.SetStatusTrack(DeviceID, GUID, ListedTillTheEnd, DateTime.Now);
            }
            catch (Exception ex)
            {
            }
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Net;

[assembly: Xamarin.Forms.Dependency(typeof(OwnRadio.Client.Droid.Track))]
namespace OwnRadio.Client.Droid
{
    [Register("Track")]

    class Track : IGetTrack
    {
        public Track()
        { }

        public String GetTrackByID(String TrackID)
        {
            Uri downloadURL = new Uri("http://ownradio.ru/api/track/GetTrackByID/" + TrackID);
            WebClient webClient = new WebClient();
            try
            {
                String fileName = Path.GetTempFileName();
                fileName = fileName.Replace(".tmp", ".mp3");
                webClient.DownloadFile(downloadURL, fileName);
                webClient.Dispose();
                if (File.Exists(fileName) == false)
                {
                    throw new Exception("Не смогли скачать файл с id = " + TrackID);
                }
                return fileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
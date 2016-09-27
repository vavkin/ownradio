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
using System.Net;
using System.IO;

namespace OwnRadio.Client.Droid
{
    class StatusTrack : ISetStatusTrack
    {
        public StatusTrack() { }

        public void SetStatusTrack(String DeviceID, String TrackID, int IsListen, DateTime DateTimeListen)
        {
            if (TrackID == "-1") return;

            Uri URLRequest = new Uri("http://ownradio.ru/api/track/SetStatusTrack/" + DeviceID + "," + TrackID + "," + IsListen + "," + DateTimeListen.ToString("dd.MM.yyyy HH:mm:sszz"));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLRequest);
            request.UserAgent = "OwnRadioMobileClient";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            String str;
            using (StreamReader stream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                str = stream.ReadToEnd();
                stream.Close();
            }
            response.Close();
        }
    }
}
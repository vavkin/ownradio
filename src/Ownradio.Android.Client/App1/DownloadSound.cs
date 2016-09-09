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

namespace ownradio
{
    class DownloadSound
    {
        public string downloadSound(Uri url)
        {
            string fileName = Path.GetTempFileName();
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, fileName);
            webClient.Dispose();
            if (File.Exists(fileName))
            {
                return fileName;
            }
            else
            {
                return "error";//!!!
            }
        }
    }
}
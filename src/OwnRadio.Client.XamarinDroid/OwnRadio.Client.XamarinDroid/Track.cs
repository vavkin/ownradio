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

namespace OwnRadio
{
	class Track : IGetTrack
	{
		public Track()
		{ }

		public String GetTrackByID(String TrackID)
		{
			Uri downloadURL = new Uri("http://10.10.0.45:5000/api/track/GetTrackByID/" + TrackID);
			WebClient webClient = new WebClient();
			String fileName = Path.GetTempFileName();
			fileName = fileName.Replace(".tmp", ".mp3");
			webClient.DownloadFile(downloadURL, fileName);
			webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
			webClient.Dispose();
			if (File.Exists(fileName) == false)
			{
				throw new Exception("Не смогли скачать файл с id = " + TrackID);
			}
			return fileName;
		}

		private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
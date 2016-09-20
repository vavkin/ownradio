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

namespace OwnRadio
{
	class NextTrackID : IGetNextTrackID
	{
		public NextTrackID() { }
		/// <summary>
		/// Получает ID следующего к проигрыванию трека
		/// </summary>
		/// <param name="DeviceID">ID устройства</param>
		/// <returns>ID следующего трэка</returns>
		public String GetNextTrackID(String DeviceID, out String Method)
		{
			Uri URLRequest = new Uri("http://10.10.0.45:5000/api/track/GetNextTrackID/" + DeviceID);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLRequest);
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			String str;
			using (StreamReader stream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
			{
				str = stream.ReadToEnd();
				stream.Close();
			}
			response.Close();

			//String searchString = "\\\"";
			//int startIndex = str.IndexOf(searchString) + searchString.Length;
			//str.Substring(startIndex, str.Length - searchString.Length);
			//searchString = "\\\"";
			//int endIndex = str.IndexOf(searchString);
			//str = str.Substring(startIndex, endIndex - startIndex);
			String trackID = str.Substring(1, 36);
			Method = "Method";//узнать, как выглядит в response
			return trackID;
		}
	}
}
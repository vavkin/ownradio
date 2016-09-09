using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
//using System.Net.Http;
//using Android.Net;
using System.IO;
using NLog;
using System.Collections.Generic;
using System.Net;

namespace ownradio
{
	[Activity(Label = "musicplay", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		bool PlayPauseFlag = false;
		bool PlayerExistFlag = false; // выбрана ли песня
		MediaPlayer _player;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			var status = FindViewById<TextView>(Resource.Id.statusBar);
			var username = FindViewById<EditText>(Resource.Id.txtUsername);
			var btnPlay = FindViewById<ImageButton>(Resource.Id.Play);
			var btnFwd = FindViewById<ImageButton>(Resource.Id.Fwd);
			var btnExit = FindViewById<Button>(Resource.Id.Exit);

			btnFwd.Enabled = false;

			string str;
			List<string> PlayList = new List<string>();
			bool listedTillTheEnd = false;
			String GUID = "-1";//for the 1st request
			//Uri url = new Uri("http://radio.redoc.ru/api/TrackSource/Play?trackId=fe325f78-1688-4bf7-a339-f8f28532bff9");
			Uri URLRequest;
			String fileName;

			//не получится одновременно грузить с сервера по несколько файлов и в том же запросе передавать флаг дослушал/нет//


			//////

			/*		URLRequest = new Uri("http://radio.redoc.ru/api/TrackSource/NextTrack?userId=297f55b4-d42c-4e30-b9d7-a802e7b7eed9&lastTrackId=" + GUID + "&lastTrackMethod=новых&listedTillTheEnd=" + listedTillTheEnd);
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLRequest);
					request.Method = "GET";
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					using (StreamReader stream = new StreamReader(
						response.GetResponseStream(), System.Text.Encoding.UTF8))
					{
						//status.Text = stream.ReadToEnd();
						str = stream.ReadToEnd();
						stream.Close();
					}
					response.Close();
					//status.Text += str;
					//string str = "<NextTrackResponse><Method>новых</Method><TrackId>9e8b3970-cfaf-4f61-a8b6-bdd42402b362</TrackId></NextTrackResponse>"; //response.ToString();
					String searchString = "\"TrackId\":\"";
					int startIndex = str.IndexOf(searchString) + searchString.Length;
					searchString = "\",\"Method\"";
					int endIndex = str.IndexOf(searchString);
					GUID = str.Substring(startIndex, endIndex - startIndex);
					//status.Text += "\n" + substring;
					Uri downloadURL = new Uri("http://radio.redoc.ru/api/TrackSource/Play?trackId=" + GUID);

					//try
					//{
					WebClient webClient = new WebClient();
					string fileName = Path.GetTempFileName();
					webClient.DownloadFile(downloadURL, fileName);
					webClient.Dispose();
					if (File.Exists(fileName))
					{

					}
					else
					{

					}
					PlayList.Add(fileName);


					/////
					*/


			//}
			//catch (Exception e)
			//{
			//    new AlertDialog.Builder(this)
			//            .SetNegativeButton("OK", (sender, args) =>
			//            {
			//            })
			//            .SetMessage(e.ToString())
			//            .SetTitle("Error")
			//            .Show();
			//}

			Dictionary<int, int> sounds = new Dictionary<int, int>();
			sounds.Add(Resource.Raw.soad_sky_is_over, Resource.Raw.piknik_nemnogo_ogna);
			sounds.Add(Resource.Raw.piknik_nemnogo_ogna, Resource.Raw.godsmack_bleeding_me);
			sounds.Add(Resource.Raw.godsmack_bleeding_me, Resource.Raw.soad_sky_is_over);

			int idSound = Resource.Raw.soad_sky_is_over;//как получить первый элемент словаря? 

			btnPlay.Click += delegate
			{
				if (!PlayerExistFlag)
				{
					///////

					URLRequest = new Uri("http://radio.redoc.ru/api/TrackSource/NextTrack?userId=297f55b4-d42c-4e30-b9d7-a802e7b7eed9&lastTrackId=" + GUID + "&lastTrackMethod=новых&listedTillTheEnd=" + listedTillTheEnd);
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLRequest);
					request.Method = "GET";
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
					using (StreamReader stream = new StreamReader(
						response.GetResponseStream(), System.Text.Encoding.UTF8))
					{
						//status.Text = stream.ReadToEnd();
						str = stream.ReadToEnd();
						stream.Close();
					}
					response.Close();
					//status.Text += str;
					//string str = "<NextTrackResponse><Method>новых</Method><TrackId>9e8b3970-cfaf-4f61-a8b6-bdd42402b362</TrackId></NextTrackResponse>"; //response.ToString();
					String searchString = "\"TrackId\":\"";
					int startIndex = str.IndexOf(searchString) + searchString.Length;
					searchString = "\",\"Method\"";
					int endIndex = str.IndexOf(searchString);
					GUID = str.Substring(startIndex, endIndex - startIndex);
					//status.Text += "\n" + substring;
					Uri downloadURL = new Uri("http://radio.redoc.ru/api/TrackSource/Play?trackId=" + GUID);

					//try
					//{
					WebClient webClient = new WebClient();
					fileName = Path.GetTempFileName();
					webClient.DownloadFile(downloadURL, fileName);
					webClient.Dispose();
					if (File.Exists(fileName))
					{

					}
					else
					{

					}
					PlayList.Add(fileName);

					///

					Toast.MakeText(this, "SetDataSource", ToastLength.Short).Show();
					PlayerExistFlag = true;
					_player = new MediaPlayer();

					//var fd = Resources.OpenRawResourceFd(idSound);
					//_player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
					_player.SetDataSource(fileName);
					_player.Prepared += (s, e) =>
					{
						_player.Start();
					};
					_player.Prepare();
					btnFwd.Enabled = true;



				}

				if (PlayPauseFlag)
				{
					Toast.MakeText(this, "Нажата кнопка пауза, флаг=" + PlayPauseFlag, ToastLength.Short).Show();

					if (_player.IsPlaying)
						_player.Pause();
					btnPlay.SetBackgroundResource(Resource.Drawable.pause);
				}
				else
				{
					Toast.MakeText(this, "Нажата кнопка проиграть, флаг=" + PlayPauseFlag, ToastLength.Short).Show();
					if (!_player.IsPlaying)
					{
						_player.Start();
					}

					btnPlay.SetBackgroundResource(Resource.Drawable.play);
				}

				PlayPauseFlag = !PlayPauseFlag;
			};

			btnFwd.Click += delegate
			{
				Toast.MakeText(this, "Нажата кнопка пропустить", ToastLength.Short).Show();
				PlayerExistFlag = false;
				PlayPauseFlag = false;
				//status.Text += "Button Forward is pressed";
				_player.Stop();
				_player.Release();
				idSound = sounds[idSound];
				btnPlay.CallOnClick();
			};
			btnExit.Click += delegate
			{
				//File.Delete(fileName);
				//DirectoryInfo dirInfo = new DirectoryInfo();
				//foreach (FileInfo file in dirInfo.GetFiles())
				//{
				//	file.Delete();
				//}
				//Toast.MakeText(this, "Временный файл удален", ToastLength.Short).Show();
				System.Environment.Exit(0);
			};
		}
	}


}


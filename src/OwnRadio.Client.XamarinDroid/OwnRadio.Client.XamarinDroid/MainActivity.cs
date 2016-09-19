using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.IO;
using System.Collections.Generic;
using System.Net;
using SQLite;
using Android.Preferences;

namespace OwnRadio
{
	[Activity(Label = "OwnRadio", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		bool PlayerExistFlag = false; // выбрана ли песня
		String FileName;
		String GUID = "-1";//for the 1st request
		String DeviceID = "297f55b4-d42c-4e30-b9d7-a802e7b7eed9";
		String Method = "новых"; // сделать список имеющихся
		bool ListedTillTheEnd = false;
		MediaPlayer	Player = new MediaPlayer();
		ISetStatusTrack StatusTrack = new StatusTrack();
		ISQLite db = new SQLite_Android();

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			var status = FindViewById<TextView>(Resource.Id.TrackInfo);
			var textDeviceID = FindViewById<EditText>(Resource.Id.EditTextDeviceID);
			var textUserID = FindViewById<EditText>(Resource.Id.EditTextUserID);
			var textName = FindViewById<EditText>(Resource.Id.EditTextName);
			var btnPlay = FindViewById<Button>(Resource.Id.Play);
			var btnNext = FindViewById<Button>(Resource.Id.Next);
			var btnSaveName = FindViewById<Button>(Resource.Id.SaveName);


			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
			String deviceID = preferences.GetString("DeviceID", "");
			if (deviceID != "")
			{
				DeviceID = deviceID;
			}
			else
			{
				DeviceID = Guid.NewGuid().ToString();
				var editor = preferences.Edit();
				editor.PutString("DeviceID", DeviceID);
				editor.Commit();
			}

			String userName = preferences.GetString("UserName", "");
			if (userName != "")
			{
				textName.Text = userName;
			}

			textDeviceID.Text = DeviceID;

			btnPlay.Click += BtnPlay_Click;
			btnNext.Click += BtnNext_Click;
			btnSaveName.Click += BtnSaveName_Click;
		}

		private void BtnSaveName_Click(object sender, EventArgs e)
		{
			var textName = FindViewById<EditText>(Resource.Id.EditTextName);
			ISharedPreferences preferences = PreferenceManager.GetDefaultSharedPreferences(this);
			var editor = preferences.Edit();
			editor.PutString("UserName", textName.Text);
			editor.Commit();
		}

		public void TrackPlay()
		{
			var trackInfo = FindViewById<TextView>(Resource.Id.TrackInfo);
			var btnPlay = FindViewById<Button>(Resource.Id.Play);
			if (!PlayerExistFlag)
			{
				var tvDeviceID = FindViewById<EditText>(Resource.Id.EditTextDeviceID);
				//btnPlay.SetBackgroundResource(Resource.Drawable.play);
				DeviceID = tvDeviceID.Text;
				IGetNextTrackID NextTrack = new NextTrackID();
				IGetTrack Track = new Track();
				try
				{
					GUID = NextTrack.GetNextTrackID(DeviceID, GUID, Method, ListedTillTheEnd);

					if (File.Exists(FileName))
					{
						File.Delete(FileName);
					}

					FileName = Track.GetTrack(GUID);
					Toast.MakeText(this, "SetDataSource", ToastLength.Short).Show();
					PlayerExistFlag = true;
					Player.Reset();
					if (File.Exists(FileName) == false)
					{
						throw new Exception("File not Exists");
					}
					Player.SetDataSource(FileName);
					Player.Prepared += (s, e) =>
					{
						Player.Start();
					};
					Player.Prepare();
					Player.Completion += Player_Completion;
					MediaMetadataRetriever mMediaMetaDataRetriever = new MediaMetadataRetriever();
					mMediaMetaDataRetriever.SetDataSource(FileName);
					String titleName = mMediaMetaDataRetriever.ExtractMetadata(MetadataKey.Title);
					String artistName = mMediaMetaDataRetriever.ExtractMetadata(MetadataKey.Artist);
					if (titleName == null) titleName = "Unknown";
					if (artistName == null) artistName = "Unknown";
					trackInfo.Text += artistName + " - " + titleName + "\n";
				}
				catch (Exception ex)
				{
					Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
					PlayerExistFlag = false;
				}
			}
			else if (Player.IsPlaying)
			{
				try
				{
					Toast.MakeText(this, "Пауза", ToastLength.Short).Show();
					Player.Pause();
					//btnPlay.SetBackgroundResource(Resource.Drawable.pause);
				}
				catch (Exception ex)
				{
					Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
				}
			}
			else if (!Player.IsPlaying)
			{
				try
				{
					Toast.MakeText(this, "Проигрывание", ToastLength.Short).Show();
					Player.Start();
					//btnPlay.SetBackgroundResource(Resource.Drawable.play);
				}
				catch (Exception ex)
				{
					Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
				}
			}
		}

		private void BtnPlay_Click(object sender, EventArgs e)
		{
			Toast.MakeText(this, "Нажата кнопка проиграть/пауза", ToastLength.Short).Show();
			TrackPlay();
		}

		private void BtnNext_Click(object sender, EventArgs e)
		{
			Toast.MakeText(this, "Нажата кнопка пропустить", ToastLength.Short).Show();
			PlayerExistFlag = false;
			if (Player.IsPlaying)
				Player.Stop();
			ListedTillTheEnd = false;
			StatusTrack.SetStatusTrack(DeviceID, GUID, ListedTillTheEnd, DateTime.Now);
			TrackPlay();
		}

		private void Player_Completion(object sender, EventArgs e)
		{
			Toast.MakeText(this, "Запущена следующая песня", ToastLength.Short).Show();
			PlayerExistFlag = false;
			ListedTillTheEnd = true;
			StatusTrack.SetStatusTrack(DeviceID, GUID, ListedTillTheEnd, DateTime.Now);
			TrackPlay();
		}

		private void BtnExit_Click(object sender, EventArgs e)
		{
			DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(Path.GetTempFileName()));
			Player.Release();
			System.Environment.Exit(0);
		}
	}
}


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

namespace ownradio
{
	[Activity(Label = "OwnRadio", MainLauncher = true, Icon = "@drawable/icon")]
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

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			var status = FindViewById<TextView>(Resource.Id.statusBar);
			var tvDeviceID = FindViewById<EditText>(Resource.Id.txtUsername);
			var btnPlay = FindViewById<ImageButton>(Resource.Id.Play);
			var btnFwd = FindViewById<ImageButton>(Resource.Id.Fwd);
			var btnExit = FindViewById<Button>(Resource.Id.Exit);

			tvDeviceID.Text = DeviceID;

			btnPlay.Click += BtnPlay_Click;

			btnFwd.Click += BtnFwd_Click;

			btnExit.Click += BtnExit_Click;
		}

		public void TrackPlay()
		{
			var btnPlay = FindViewById<ImageButton>(Resource.Id.Play);
			if (!PlayerExistFlag)
			{
				IGetNextTrackID NextTrack = new NextTrackID();
				IGetTrack Track = new Track();
				GUID = NextTrack.GetNextTrackID(DeviceID, GUID, Method, ListedTillTheEnd);
				FileName = Track.GetTrack(GUID);
				Toast.MakeText(this, "SetDataSource", ToastLength.Short).Show();
				PlayerExistFlag = true;
				//Player = new MediaPlayer();
				Player.Reset();
				Player.SetDataSource(FileName);
				Player.Prepared += (s, e1) =>
				{
					Player.Start();
				};
				Player.Prepare();
				Player.Completion += Player_Completion;
			}
			if (Player.IsPlaying)
			{
				Toast.MakeText(this, "Пауза", ToastLength.Short).Show();
				Player.Pause();
				btnPlay.SetBackgroundResource(Resource.Drawable.pause);
			}
			else
			{
				Toast.MakeText(this, "Проигрывание", ToastLength.Short).Show();
				Player.Start();
				btnPlay.SetBackgroundResource(Resource.Drawable.play);
			}
		}




		private void BtnPlay_Click(object sender, EventArgs e)
		{
			Toast.MakeText(this, "Нажата кнопка проиграть/пауза", ToastLength.Short).Show();
			TrackPlay();
		}

		private void BtnFwd_Click(object sender, EventArgs e)
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
			foreach (FileInfo file in dirInfo.GetFiles())
			{
				file.Delete();
			}
			System.Environment.Exit(0);
		}
	}
}


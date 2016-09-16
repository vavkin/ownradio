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

namespace ownradio
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
	
			var status = FindViewById<TextView>(Resource.Id.statusBar);
			var tvDeviceID = FindViewById<EditText>(Resource.Id.txtDeviceID);
			var btnPlay = FindViewById<ImageButton>(Resource.Id.Play);
			var btnNext = FindViewById<ImageButton>(Resource.Id.Next);
			var btnExit = FindViewById<Button>(Resource.Id.Exit);
			var btnShowDB = FindViewById<Button>(Resource.Id.ShowDB);
			DeviceID = Build.Serial;
			tvDeviceID.Text = DeviceID;

			btnPlay.Click += BtnPlay_Click;
			btnNext.Click += BtnNext_Click;
			btnExit.Click += BtnExit_Click;
			btnShowDB.Click += BtnShowDB_Click;

		}

		///
		//[Table("history")]
		//public class history
		//{
		//	[PrimaryKey, Column("Id"), AutoIncrement]
		//	public string id { get; set; }
		//	public string trackid { get; set; }
		//	public string datetimelisten { get; set; }
		//	public int listenyesno { get; set; }
		//}

		private void BtnShowDB_Click(object sender, EventArgs e)
		{
			var status = FindViewById<TextView>(Resource.Id.statusBar);

			try
			{
				string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				var path = Path.Combine(dbPath, "ownradio.db");
				// создаем подключение
				//var db = new SQLiteConnection(path);
				db.GetConnection("ownradio.db");
				//db.CreateTableTrack(GUID, DateTime.Now.ToString());

				//var sqlCommand = db.CreateCommand("SELECT * FROM sqlite_master");
				//var res = sqlCommand.ExecuteScalar<string>();
				//var toHistory = new history { id = Guid.NewGuid().ToString(), trackid = GUID, datetimelisten = DateTime.Now.ToString(), listenyesno = Convert.ToInt32(ListedTillTheEnd) };
				//db.CreateTable<history>();
				//db.Insert(toHistory);
				//var info = db.GetTableInfo("history");
				//var text = db.Query<history>("SELECT * FROM history");
				//db.Close();
			}
			catch(Exception ex)
			{
				status.Text += ex;
			}
		}

		public void TrackPlay()
		{
			var btnPlay = FindViewById<ImageButton>(Resource.Id.Play);
			if (!PlayerExistFlag)
			{
				var tvDeviceID = FindViewById<EditText>(Resource.Id.txtDeviceID);
				btnPlay.SetBackgroundResource(Resource.Drawable.play);
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
					btnPlay.SetBackgroundResource(Resource.Drawable.pause);
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
					btnPlay.SetBackgroundResource(Resource.Drawable.play);
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


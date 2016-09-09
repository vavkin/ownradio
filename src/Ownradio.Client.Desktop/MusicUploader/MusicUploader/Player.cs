using NLog;
using System;
using WMPLib;

namespace OwnRadio.DesktopPlayer
{
	class Player : IDisposable
	{              
		private TrackSource trackSource;
		private NextTrackResponse currentResponce;
		private Settings settings;
		public bool IsPause { private set; get; }
		private WindowsMediaPlayer wplayer;
		private Logger log;

		public Player(Logger logger)
		{
			log = logger;
			
			try
			{
				settings = new Settings(logger);
				trackSource = new TrackSource(settings, logger);
				IsPause = false;
				wplayer = new WindowsMediaPlayer();
				wplayer.PlayStateChange += Wplayer_PlayStateChange;
				start();
			}
			catch (Exception ex)
			{
				logger.Error(ex.Message);
			}
		}

		private void Wplayer_PlayStateChange(int NewState)
		{
			if (NewState == (int)WMPPlayState.wmppsMediaEnded)
			{             
				next(true);                
			}
			if (NewState == (int)WMPPlayState.wmppsStopped)
			{
				wplayer.controls.play();                
			}
		}

		private void start()
		{
			try
			{
				currentResponce = trackSource.FirstTrack();        
				wplayer.URL = trackSource.Play(currentResponce.TrackId);
			}   
			catch(Exception ex)
			{
				log.Error(ex.Message);
			}         
		}

		public void Play()
		{
			if (IsPause)
			{
				wplayer.controls.play();
				IsPause = false;               
			}          
		}

		public void Pause()
		{
			if (!IsPause)
			{
				wplayer.controls.pause();
				IsPause = true;
			}
		}

		public void Next()
		{
			next(false);
		}

		private void next (bool listedTillTheEnd)
		{
			try
			{
				currentResponce = trackSource.NextTrack(currentResponce.TrackId, currentResponce.Method, listedTillTheEnd);
				wplayer.URL = trackSource.Play(currentResponce.TrackId);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
			}
		}

		public void Dispose()
		{
			wplayer?.close();
		}
	}
}

using NLog;
using System.Net.Http;
using System.Threading.Tasks;

namespace OwnRadio.DesktopPlayer
{    
	class TrackSource
	{
		private string userId;
		private string address;
		Logger log;

		public TrackSource(Settings settings, Logger logger)
		{
			address = settings.serverAddress;
			userId = settings.userId;
			log = logger;
		}
	   
		public NextTrackResponse FirstTrack()
		{
			string url = getNextTrackRequest("-1", "", false);
			return getNextTrack(url);
		}

		public NextTrackResponse NextTrack(string lastTrackId, string lastTrackMethod, bool listedTillTheEnd)
		{
			string url = getNextTrackRequest(lastTrackId, lastTrackMethod, listedTillTheEnd);
			return getNextTrack(url);
		}

		public string Play(string trackId)
		{
			return string.Format("{0}/api/TrackSource/Play?trackId={1}", address, trackId);
		}

		private string getNextTrackRequest(string lastTrackId, string lastTrackMethod, bool listedTillTheEnd)
		{
			return string.Format("{0}/api/TrackSource/NextTrack?userId={1}&lastTrackId={2}&lastTrackMethod={3}&listedTillTheEnd={4}"
				, address, userId, lastTrackId, lastTrackMethod, listedTillTheEnd.ToString().ToLower());
		}

		private NextTrackResponse getNextTrack(string url)
		{
			NextTrackResponse result = null;
			bool received = false;
			using (HttpClient httpClient = new HttpClient())
			{                
				Task taskReceive = httpClient.GetAsync(url).ContinueWith(task =>
				{
					if (task.Status == TaskStatus.RanToCompletion)
					{
						var response = task.Result;
						if (response.IsSuccessStatusCode)
						{
							result = response.Content.ReadAsAsync<NextTrackResponse>().Result;
							received = result != null;
						}
					}
				});
				taskReceive.Wait();
			}
			log.Debug(received ? "получен трек" : "не получен трек");
			return result;
		}


	}
}

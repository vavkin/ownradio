using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Net.Http.Headers;

namespace Radio.Web.Controllers
{
    [Route("api/[controller]")]
    public class TrackController : Controller
    {
        private readonly IApplicationEnvironment appEnvironment;

        public TrackController(IApplicationEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        [HttpGet("getNextTrack")]
        public object GetNextTrack(string userGuid)
        {
            return new { hash = "hash", trackId = 1 };
        }

        [HttpGet("getAudio")]
        public IActionResult GetAudio(int trackId, string hash)
        {
            var path = Path.Combine(appEnvironment.ApplicationBasePath, "wwwroot\\content\\audio.mp3");
            var stream = System.IO.File.OpenRead(path);
            Response.ContentLength = stream.Length;

            return new FileStreamResult(stream, "audio/mpeg");
        }

        [HttpPost("setTrackStatus")]
        public void SetTrackStatus(int trackId, string userGuid, bool status)
        {
        }
    }
}

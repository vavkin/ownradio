using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OwnRadio.Web.Api.Infrastructure;
using OwnRadio.Web.Api.Models;
using System;

namespace OwnRadio.Web.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	public class TrackController : Controller
    {
		public Settings settings { get; }

		public TrackController(IOptions<Settings> settings)
		{
			this.settings = settings.Value;
		}
		
		// Возвращает Guid следующего трека для заданного идентификатором устройства
		// GET api/track/GetNextTrackID/12345678-1234-1234-1234-123456789012
		[HttpGet("{deviceID}")]
		public Guid GetNextTrackID(Guid deviceID)
		{
			var track = new Track(settings.connectionString);
			return track.GetNextTrackID(deviceID);
		}

		// Возвращает трек (поток audio/mpeg) по его идентификатору
		// GET api/track/GetNextTrackID/12345678-1234-1234-1234-123456789012
		[HttpGet("{trackID}")]
		public IActionResult GetTrackByID(Guid trackID)
		{
			// Получаем путь к треку
			var track = new Track(settings.connectionString);
			var path = track.GetTrackPath(trackID);
			// Создаем поток из трека
			var stream = System.IO.File.OpenRead(path);
			Response.ContentLength = stream.Length;
			// Возвращаем поток audio/mpeg
			return new FileStreamResult(stream, "audio/mpeg");
		}
	}
}

using Microsoft.AspNetCore.Mvc;

namespace OwnRadio.Web.Api.Controllers
{
	[Route("api/[controller]")]
	public class TrackController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

		// GET api/track/5
		[HttpGet("{id}")]
		public string GetNextTrackID(int id)
		{
			return "value";
		}

	}
}

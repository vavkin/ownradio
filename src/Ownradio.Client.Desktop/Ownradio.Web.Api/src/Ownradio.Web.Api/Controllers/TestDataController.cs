using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ownradio.Models;
using Ownradio.Web.Api.Infrastructure;

namespace ownradio.Controllers
{
	[Route("api/[controller]")]
	public class TestDataController : Controller
	{
		private readonly ConcurrentBag<TestData> _all;
		public TestDataController()
		{
			_all = new ConcurrentBag<TestData>
			{
				new TestData
				{
					Id = 1,
					Value = "test1"
				},
				new TestData
				{
					Id = 5,
					Value = "test5"
				}
			};
		}
		// GET api/testdata
		[HttpGet]
		public IActionResult Get()
		{
			return Json(_all);
		}

		// GET api/testdata/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var value = _all.FirstOrDefault(x => x.Id == id);
			return Json(value);
		}

		// POST api/testdata
		[MimeMultipart]
		public void Post() //([FromBody]string value)
		{
			var x = "text";
		}

		// PUT api/testdata/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/testdata/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}

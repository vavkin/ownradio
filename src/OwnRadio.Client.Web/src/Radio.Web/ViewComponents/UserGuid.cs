using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Primitives;
using Radio.Web.Models;
using System;
using System.Linq;

namespace Radio.Web.ViewComponents
{
    public class UserGuid : ViewComponent
    {
        public const string UserGuidCookie = "UserGuid";

        private readonly UserManager<ApplicationUser> _userManager;

        public UserGuid(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var cookie = Request.Cookies[UserGuidCookie];
            var guid = string.Empty;

            if (StringValues.IsNullOrEmpty(cookie))
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = _userManager.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                    guid = user.UserGuid;
                }
                else
                {
                    guid = Guid.NewGuid().ToString();
                }
                
                HttpContext.Response.Cookies.Append(UserGuidCookie, guid, new Microsoft.AspNet.Http.CookieOptions() { HttpOnly = true });
            }
            else
            {
                guid = cookie.First();
            }

            return View("Default", guid);
        }
    }
}

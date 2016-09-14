using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Radio.Web.Models;
using Radio.Web.Services;
using Radio.Web.ViewComponents;
using System.Linq;

namespace Radio.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View("ExternalLoginFailure");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                Response.Cookies.Delete(UserGuid.UserGuidCookie);
                Response.Cookies.Append(UserGuid.UserGuidCookie, user.UserGuid, new Microsoft.AspNet.Http.CookieOptions() { HttpOnly = true });
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                var userGuid = Request.Cookies[UserGuid.UserGuidCookie].First();
                var userName = info.ExternalPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = info.ExternalPrincipal.FindFirstValue(ClaimTypes.Email) ?? userName;
                var pictureUrl = info.ExternalPrincipal.FindFirstValue("picture");

                var existing = _userManager.Users.SingleOrDefault(x => x.UserGuid == userGuid || x.UserName == userName || x.Email == email);
                if (existing == null)
                {
                    var user = new ApplicationUser { UserName = userName, Email = email, UserGuid = userGuid, PictureUrl = pictureUrl };
                    var registrationResult = await _userManager.CreateAsync(user);
                    if (registrationResult.Succeeded)
                    {
                        registrationResult = await _userManager.AddLoginAsync(user, info);
                        if (registrationResult.Succeeded)
                        {
                            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);
                            _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(existing.PictureUrl))
                    {
                        existing.PictureUrl = pictureUrl;
                        await _userManager.UpdateAsync(existing);
                    }

                    var registrationResult = await _userManager.AddLoginAsync(existing, info);
                    if (registrationResult.Succeeded)
                    {
                        await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);
                        _logger.LogInformation(6, "User added an account using {Name} provider.", info.LoginProvider);
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                

                return View("ExternalLoginFailure");
            }
        }
    }
}

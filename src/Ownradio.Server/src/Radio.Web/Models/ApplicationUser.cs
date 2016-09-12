using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Radio.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string UserGuid { get; set; }

        public string PictureUrl { get; set; }
    }
}

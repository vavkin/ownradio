using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radio.Web.Models;
using Radio.Web.Services;
using Radio.Web.Config;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Authentication.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.WebUtilities;
using Microsoft.AspNet.Identity;
using System;

namespace Radio.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddUserSecrets();
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddOptions();
            services.Configure<AuthenticationConfig>(Configuration.GetSection("AuthenticationConfig"));

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters += " _-@йцукенгшщзхъфывапролджэячсмитьбюЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";
                    options.User.RequireUniqueEmail = false;
                    options.Cookies.ApplicationCookie.SlidingExpiration = true;
                    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(365);
                    options.Cookies.ExternalCookie.SlidingExpiration = true;
                    options.Cookies.ExternalCookie.ExpireTimeSpan = TimeSpan.FromDays(365);
                    options.SecurityStampValidationInterval = TimeSpan.FromDays(365);
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
            
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AuthenticationConfig> authConfig, ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            context.Database.Migrate();

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseFacebookAuthentication(options =>
            {
                options.AppId = authConfig.Value.Facebook.AppId;
                options.AppSecret = authConfig.Value.Facebook.AppSecret;
                options.Scope.Add("public_profile");
                options.Scope.Add("email");
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = ctx =>
                    {
                        var pic = ctx.User.SelectToken("picture.data.url").ToString();
                        ctx.Identity.AddClaim(new Claim("picture", pic));
                        return Task.FromResult(0);
                    }
                };
                options.UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name,location,picture";
            });

            app.UseOAuthAuthentication(options =>
            {
                options.DisplayName = "VK";
                options.AuthenticationScheme = "VK";
                options.ClientId = authConfig.Value.Vk.ClientId;
                options.ClientSecret = authConfig.Value.Vk.ClientSecret;
                options.CallbackPath = new PathString("/signin-vk");
                options.AuthorizationEndpoint = "https://oauth.vk.com/authorize";
                options.TokenEndpoint = "https://oauth.vk.com/access_token";
                options.UserInformationEndpoint = "https://api.vk.com/method/users.get";
                options.Scope.Add("email");
                options.Scope.Add("offline");
                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async ctx =>
                    {
                        var userId = ctx.TokenResponse.Response.SelectToken("user_id").ToString();
                        var url = QueryHelpers.AddQueryString(ctx.Options.UserInformationEndpoint, "user_id", userId);
                        url = QueryHelpers.AddQueryString(url, "access_token", ctx.AccessToken);
                        url = QueryHelpers.AddQueryString(url, "fields", "photo_50");
                        var request = new HttpRequestMessage(HttpMethod.Get, url);

                        var response = await ctx.Backchannel.SendAsync(request, ctx.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                        var email = ctx.TokenResponse.Response.SelectToken("email");

                        if (email != null)
                        {
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.Email, email.ToString(), ClaimValueTypes.String, ctx.Options.ClaimsIssuer));
                        }

                        var fullName = user.SelectToken("response[0].first_name").Value<string>() + " " + user.SelectToken("response[0].last_name").Value<string>();
                        if (!string.IsNullOrEmpty(fullName))
                        {
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.Name, fullName, ClaimValueTypes.String, ctx.Options.ClaimsIssuer));
                        }

                        var picture = user.SelectToken("response[0].photo_50").Value<string>();
                        if (!string.IsNullOrEmpty(picture))
                        {
                            ctx.Identity.AddClaim(new Claim("picture", picture, ClaimValueTypes.String, ctx.Options.ClaimsIssuer));
                        }

                        ctx.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, ctx.Options.ClaimsIssuer));
                    }
                };
            });

            app.UseMvcWithDefaultRoute();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

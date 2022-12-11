using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS;
using OnlineBanking.Models;

namespace OnlineBanking
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(option =>
            {
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "AuthToken";
                options.AccessDeniedPath= "/Home/AccessDenied";
                options.LoginPath= "/Home/Login";

            });

            services.AddAuthorization(option =>
            {

                option.AddPolicy("UserPolicy", policy => policy.RequireClaim("Role", "User"));
                option.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin"));

            });

            //services.AddIdentity<IdentityUser, IdentityRole>().AddRoles<Role>();

            
        }
    }
}

using Maqha.Repository.Data;
using Maqha.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Maqha.Extensions
{
    public static class AddServiceExtension
    {
        public static void AddService(this IServiceCollection Service,IConfiguration Configuration)
        {
            //Register MaqhaDbContext
            Service.AddDbContext<MaqhaDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaqhaDbConnection")); // Replace with your actual connection string
            });
            //Register AppIdentityContext
            Service.AddDbContext<AppIdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaqhaDbConnection")); // Replace with your actual connection string
            });
            //Register AppIdentitUser and AppIdentityRole
            Service.AddIdentityCore<AppIdentityUser>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 6;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
            }).AddRoles<AppIdentityRole>()
    .AddEntityFrameworkStores<AppIdentityContext>()
    .AddSignInManager<SignInManager<AppIdentityUser>>()
    .AddDefaultTokenProviders();
        }
    }
}

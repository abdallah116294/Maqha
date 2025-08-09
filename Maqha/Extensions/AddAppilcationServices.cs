using Maqha.Core.IRepository;
using Maqha.Core.Services;
using Maqha.Repository.Repository;
using Maqha.Service;
using Maqha.Utilities.Helpers;

namespace Maqha.Extensions
{
    public static class AddAppilcationServices
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection)
        {
            //Register application services here
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IRoleService, RoleService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();
            serviceCollection.AddScoped<IImageUploadService, ImageUploadService>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddAutoMapper(typeof(MappingProfile));
            serviceCollection.AddTransient<TokenHelper>();

        }
    }
}

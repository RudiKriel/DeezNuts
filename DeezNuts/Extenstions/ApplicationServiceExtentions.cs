using BLL.Interfaces;
using BLL.Managers;
using DAL.Context;
using DAL.Interfaces;
using DAL.Repositories;
using DeezNuts.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DeezNuts.Extenstions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<ApplicationDbContext>(context =>
            {
                context.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}

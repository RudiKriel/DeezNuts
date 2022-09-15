using DeezNuts.BLL.Interfaces;
using DeezNuts.BLL.Managers;
using DAL.Context;
using DAL.Interfaces;
using DeezNuts.Helpers;
using Microsoft.EntityFrameworkCore;
using DAL;

namespace DeezNuts.Extenstions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<PresenceTracker>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<ApplicationDbContext>(context =>
            {
                context.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}

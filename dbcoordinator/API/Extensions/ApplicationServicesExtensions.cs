using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServicesExtensions
{
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITokenService,TokenService>();
            services.AddDbContext<DataContext>(opt => {
                opt.UseSqlite(config.GetConnectionString("SqliteConnection"));
            });
            return services;
        }
}

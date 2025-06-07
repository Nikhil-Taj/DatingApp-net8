using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationservices(this IServiceCollection services, IConfiguration config)
        {
               services.AddDbContext<DataContext>(options =>
                 options.UseSqlite(config.GetConnectionString("DefaultConnection")));
               services.AddCors();
               services.AddControllers();
               services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}

using CatMatch.Application.Services;
using CatMatch.Application.Services.CatMatch;
using Microsoft.Extensions.DependencyInjection;

namespace CatMatch.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICatMatchService, CatMatchService>();
            services.AddScoped<ICatMatchDataAccess, CatMatchDataAccess>();

            return services;
        }
    }
}

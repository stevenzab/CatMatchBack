using CatMatch.Application.Services;
using CatMatch.Application.Services.CatMatch;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

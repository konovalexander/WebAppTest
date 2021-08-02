using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, string repositoryType, string connectionString)
        {
            switch (repositoryType)
            {
                case "memory":
                    services.AddSingleton<IUserRepository, MemoryUserRepository>();
                    break;

                case "database":
                    services.AddScoped<IUserRepository, SqlUserRepository>();
                    services.AddDbContext<SqlDbContext>(options =>
                    {
                        options.UseSqlServer(connectionString);
                    });
                    break;

                default:
                    throw new ArgumentException("Unknown connection type");
            };

            return services;
        }
    }
}

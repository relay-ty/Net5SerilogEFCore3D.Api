using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net5SerilogEFCore3D.Api.Configuration;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.Configuration;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Api.Extensions.DatabasesExtensions
{
    public static class InitDatabases
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="args"></param>
        /// <param name="configuration"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static async Task ApplyDbMigrationsWithDataSeedAsync(string[] args, IConfiguration configuration, IHost host)
        {
            var listDatabaseConfiguration = configuration.GetSection(nameof(DatabaseConfiguration)).Get<List<DatabaseConfiguration>>()
                .Where(w => w.Enabled == true).OrderBy(o => o.Sort).ToList();

            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            foreach (var item in listDatabaseConfiguration)
            {
                switch (item.DbContextName)
                {
                    case nameof(SerilogDbContext):
                        {
                            if (item.ApplyDatabaseMigrations)
                                await EnsureDatabasesMigratedAsync<SerilogDbContext>(services);
                            if (item.ApplyDataSeed)
                                await EnsureSeedDataAsync<SerilogDbContext>(services);
                            break;
                        }

                    default:
                        throw new ArgumentOutOfRangeException(item.DbContextName, $"DbContext is not configured at InitDatabases.ApplyDbMigrationsWithDataSeedAsync");

                }
            };

             
        }


        /// <summary>
        /// EnsureDatabasesMigratedAsync
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static async Task EnsureDatabasesMigratedAsync<TDbContext>(IServiceProvider services) where TDbContext : DbContext
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await context.Database.MigrateAsync();
        }

        /// <summary>
        /// EnsureSeedDataAsync
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static async Task EnsureSeedDataAsync<TDbContext>(IServiceProvider services) where TDbContext : DbContext
        {
            using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await Task.Run(() => { });

            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
            //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<TRole>>();

            //var rootConfiguration = scope.ServiceProvider.GetRequiredService<IRootConfiguration>();

            //await EnsureSeedIdentityServerData(context, rootConfiguration.IdentityServerDataConfiguration);
            //await EnsureSeedIdentityData(userManager, roleManager, rootConfiguration.IdentityDataConfiguration);
        }
    }
}

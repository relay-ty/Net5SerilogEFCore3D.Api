using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Net5SerilogEFCore3D.Infrastructure.EF.SqlServer.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Register DbContexts
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        public static void RegisterSqlServerDbContexts<TDbContext>(
            this IServiceCollection services,string connectionString)
            where TDbContext :DbContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;
            //TDB from existing connection
            services.AddDbContext<TDbContext>(options => options.UseSqlServer(connectionString, optionsSql => optionsSql.MigrationsAssembly(migrationsAssembly)));
            services.AddScoped<TDbContext>();//生命周期内
        }
    }
}

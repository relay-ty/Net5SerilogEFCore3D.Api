using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts;
using System.Reflection;

namespace Net5SerilogEFCore3D.Infrastructure.EF.SqlServer
{
    public class DesignTimeSerilogDbContextFactory : IDesignTimeDbContextFactory<SerilogDbContext>
    {
        public SerilogDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=192.168.3.99\\TYMSSQLSERVER;Database=Net5SerilogEFCore3DApi;User ID=sa;Password=Phone18887152329;";

            var migrationsAssembly = typeof(DesignTimeSerilogDbContextFactory).GetTypeInfo().Assembly.GetName().Name;

            var optionsBuilder = new DbContextOptionsBuilder<SerilogDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            return new SerilogDbContext(optionsBuilder.Options);
        }
    }
}

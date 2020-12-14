using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.Configuration;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Net5SerilogEFCore3D.Infrastructure.EF.SqlServer
{
    public class DesignTimeSerilogDbContextFactory : IDesignTimeDbContextFactory<SerilogDbContext>
    {
        public SerilogDbContext CreateDbContext(string[] args)
        {
            //获取运行环境变量
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // 从 appsetting.json 中获取配置信息
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())//设置目录为当前目录     //Microsoft.Extensions.Configuration.FileExtensions                                                         
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)//appsettings 配置文件 //Microsoft.Extensions.Configuration.Json
               .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
               .Build();

            var connectionString = configuration.GetSection(nameof(DatabaseConfiguration))
                .Get<List<DatabaseConfiguration>>()//Microsoft.Extensions.Configuration.Binder
                .Where(w => w.Enabled == true && w.DbContextName == nameof(SerilogDbContext)).FirstOrDefault().ConnectionString;

            var migrationsAssembly = typeof(DesignTimeSerilogDbContextFactory).GetTypeInfo().Assembly.GetName().Name;

            var optionsBuilder = new DbContextOptionsBuilder<SerilogDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            return new SerilogDbContext(optionsBuilder.Options);
        }
    }
}

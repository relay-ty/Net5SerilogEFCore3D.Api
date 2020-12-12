using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net5SerilogEFCore3D.Api.Configuration;
using Net5SerilogEFCore3D.Infrastructure.EF.MySql.Extensions;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.Configuration;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts;
using Net5SerilogEFCore3D.Infrastructure.EF.SqlServer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Api.Extensions.ServiceExtensions
{
    /// <summary>
    /// 注册 EF Core 数据库上下文 服务
    /// </summary>
    public static class EFCoreSetup
    {
        public static void AddEFCoreSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var startupConfiguration = configuration.GetSection(nameof(StartupConfiguration)).Get<StartupConfiguration>();
            var listDatabaseConfiguration = startupConfiguration.DatabasesConfiguration.Where(w => w.Enabled == true).OrderBy(o => o.Sort).ToList();

            foreach (var item in listDatabaseConfiguration)
            {
                switch (item.ProviderType)
                {
                    case DatabaseProviderType.SqlServer:
                        {
                            switch (item.DbContextName)
                            {
                                case "SerilogDbContext":
                                    services.RegisterSqlServerDbContexts<SerilogDbContext>(item.ConnectionString);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(item.DbContextName, $"DbContext is not configured at EFCoreSetup.AddEFCoreSetup");
                            }
                            break;
                        }
                    case DatabaseProviderType.MySql:
                        {
                            switch (item.DbContextName)
                            {
                                case "SerilogDbContext":
                                    services.RegisterMySqlDbContexts<SerilogDbContext>(item.ConnectionString);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(item.DbContextName, $"DbContext is not configured at EFCoreSetup.AddEFCoreSetup");
                            }
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(item.ProviderType).ToString(), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
                }
            }
        }
    }
}

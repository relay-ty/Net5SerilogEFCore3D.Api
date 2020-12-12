using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Net5SerilogEFCore3D.Api.Configuration;
using Net5SerilogEFCore3D.Api.Extensions.DatabasesExtensions;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //获取运行环境变量
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //加载配置文件
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())//设置目录为当前目录                                                             
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)//appsettings 配置文件
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();
            //使用 Serilog 记录日志
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)//Serilog.Settings.Configuration               
                .CreateLogger();

            try
            {
                Log.Information("Host Creating... ");

                var host = CreateHostBuilder(args, configuration).Build();
                ////应用 DbMigrations DataSeed
                await InitDatabases.ApplyDbMigrationsWithDataSeedAsync(args, configuration, host);

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Host terminated unexpectedly {ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        {
            var localPort = configuration.GetSection(nameof(StartupConfiguration)).Get<StartupConfiguration>().LocalPort;
            return Host.CreateDefaultBuilder(args)
                //添加Autofac服务工厂
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                //配置 应用程序配置           
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true)// 捕捉启动异常
                    .UseStartup<Startup>().UseUrls($"http://*:{localPort}");
                })
                .UseSerilog();
        }
    }
}

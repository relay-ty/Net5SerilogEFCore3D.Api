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
            //��ȡ���л�������
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //���������ļ�
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())//����Ŀ¼Ϊ��ǰĿ¼                                                             
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)//appsettings �����ļ�
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();
            //ʹ�� Serilog ��¼��־
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)//Serilog.Settings.Configuration               
                .CreateLogger();

            try
            {
                Log.Information("Host Creating... ");

                var host = CreateHostBuilder(args, configuration).Build();
                ////Ӧ�� DbMigrations DataSeed
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
                //���Autofac���񹤳�
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                //���� Ӧ�ó�������           
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.CaptureStartupErrors(true)// ��׽�����쳣
                    .UseStartup<Startup>().UseUrls($"http://*:{localPort}");
                })
                .UseSerilog();
        }
    }
}

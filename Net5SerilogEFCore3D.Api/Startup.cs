using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Net5SerilogEFCore3D.Api.Extensions.ServiceExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //RegisterDbContexts
            services.AddEFCoreSetup(Configuration);
            services.AddAutoMapperSetup();
            // Add MediatR for Domain Events 领域命令、领域事件等注入
            // 引用包 MediatR.Extensions.Microsoft.DependencyInjection
            services.AddMediatR(typeof(Startup));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Net5SerilogEFCore3D.Api", Version = "v1" });
            });
        }
        //配置容器:注意在Program.CreateHostBuilder，添加Autofac服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Autofac处理依赖注入
            builder.RegisterModule(new AutofacModuleRegister(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Net5SerilogEFCore3D.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

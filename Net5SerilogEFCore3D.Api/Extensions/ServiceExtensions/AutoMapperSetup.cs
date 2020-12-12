using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Net5SerilogEFCore3D.Application.AutoMapper;
using System;

namespace Net5SerilogEFCore3D.Api.Extensions.ServiceExtensions
{
    /// <summary>
    /// AutoMapper 的启动服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //添加服务 AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //启动配置
            AutoMapperConfig.RegisterMappings();
        }
    }
}

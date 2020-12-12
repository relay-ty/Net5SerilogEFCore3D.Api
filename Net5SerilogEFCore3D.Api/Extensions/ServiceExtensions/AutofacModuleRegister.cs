using Autofac;
using Microsoft.Extensions.Configuration;
using Net5SerilogEFCore3D.Api.Configuration;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Infrastructure.Repositories;
using Net5SerilogEFCore3D.Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Net5SerilogEFCore3D.Api.Extensions.ServiceExtensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        private readonly string _BasePath = AppContext.BaseDirectory; //获取项目绝对路径
        private readonly IConfiguration _Configuration;

        public AutofacModuleRegister(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            /*********************注意几个生命周期问题*******************************
             * InstancePerDependency();每次都会返回一个新的实例，并且这是默认的生命周期。
             * SingleInstance();//单例，所有服务请求都将会返回同一个实例。
             * InstancePerLifetimeScope();在一个嵌套语句块中，只会返回一个实例。
             */

            #region 泛型依赖注入
            containerBuilder.RegisterGeneric(typeof(UnitOfWork<>)).As(typeof(IUnitOfWork<>)).InstancePerLifetimeScope();//注册 泛型 工作单元接口
            containerBuilder.RegisterGeneric(typeof(QueryRepository<,>)).As(typeof(IQueryRepository<,>)).InstancePerLifetimeScope();//注册 泛型 仓储
            containerBuilder.RegisterGeneric(typeof(CommandRepository<,>)).As(typeof(ICommandRepository<,>)).InstancePerLifetimeScope();//注册 泛型 仓储
            #endregion



            var startupConfiguration = _Configuration.GetSection(nameof(StartupConfiguration)).Get<StartupConfiguration>();
            var appSourceName = startupConfiguration.AppSourceName;
            #region 带有接口的服务层注入
            //var listIoc = Appsettings.App<IocSettings>("Iocs").Where(w => w.Enabled).OrderBy(o => o.Sort).ToList();
            var listIoc = new List<IocSettings>()
            {
                new IocSettings(){ DllFileName=$"{appSourceName}.Application.dll"},
                new IocSettings(){ DllFileName=$"{appSourceName}.Domain.dll"},
                new IocSettings(){ DllFileName=$"{appSourceName}.Domain.Core.dll"},
                new IocSettings(){ DllFileName=$"{appSourceName}.Infrastructure.dll"},
            };
            listIoc.ForEach(f =>
            {
                try
                {
                    //加载 DLL 程序集 
                    var loadDllFilePath = Path.Combine(_BasePath, f.DllFileName);
                    var assembly = Assembly.LoadFrom(loadDllFilePath);
                    //注册
                    containerBuilder.RegisterAssemblyTypes(assembly)
                              .AsImplementedInterfaces()
                              //.InstancePerDependency();//每次都会返回一个新的实例，并且这是默认的生命周期。
                              .InstancePerLifetimeScope(); //在一个嵌套语句块中，只会返回一个实例。
                    //.EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                    //.InterceptedBy(listAopType.ToArray());//允许将拦截器服务的列表分配给注册。
                }
                catch (Exception ex)
                {
                    var erroMessage = $"※※★※※请检查 {f.DllFileName} 是否存在※※★※※\n{ ex.Message }\n{ ex.InnerException} ";
                    //_Log.Error(erroMessage);
                    throw new Exception(erroMessage);
                }
            });
            #endregion
        }
        public class IocSettings
        {
            /// <summary>
            /// 连接字符串
            /// </summary>
            public string DllFileName { get; set; }

            /// <summary>
            /// 排序
            /// </summary>
            public int Sort { get; set; }
          
            /// <summary>
            /// 启用开关
            /// </summary>
            public bool Enabled { get; set; }
            
        }
    }
}

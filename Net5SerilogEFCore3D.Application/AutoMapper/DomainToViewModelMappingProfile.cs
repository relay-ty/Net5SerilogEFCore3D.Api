using AutoMapper;
using Net5SerilogEFCore3D.Model.DomainModels;
using Net5SerilogEFCore3D.Model.ViewModels;

namespace Net5SerilogEFCore3D.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射 //这个是领域模型 -> 视图模型的映射，是 读命令
        /// </summary>
        public DomainToViewModelMappingProfile()
        {

            CreateMap<Serilog, SerilogView>();




        }
    }
}

using AutoMapper;
using Net5SerilogEFCore3D.Domain.Commands.Serilog;
using Net5SerilogEFCore3D.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        //这里是视图模型 -> 领域命令的映射
        public ViewModelToDomainMappingProfile()
        {
            #region SerilogView
            //视图模型 -> 添加信息命令模型
            CreateMap<SerilogView, RegisterSerilogCommand>()
                .ConstructUsing(c => new RegisterSerilogCommand(c.Id,c.Message,c.Level,c.TimeStamp.ToUniversalTime(),c.Exception, c.LogEvent,c.Remarks,c.IsEnable,c.CreateTime,c.CreateUserId));
            #endregion



        }
    }
}

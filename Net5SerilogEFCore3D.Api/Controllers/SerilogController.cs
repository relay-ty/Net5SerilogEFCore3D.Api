using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Net5SerilogEFCore3D.Application.Interfaces;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Api.Controllers
{
    public class SerilogController : BaseController<SerilogController>
    {
        private readonly ISerilogService _SerilogService;
        public SerilogController(ISerilogService serilogService,
            INotificationHandler<DomainNotification> notifications, ILogger<SerilogController> logger, IMapper mapper):base(notifications,logger,mapper)
        {
            _SerilogService = serilogService;
        }

        [HttpPost]
        public async Task<MessageModel<object>> RegisterAsync(SerilogView serilogView)
        {
            // 视图模型验证
            if (!ModelState.IsValid)
                return new MessageModel<object>() { Success = ModelState.IsValid, Message = "ModelState", Data = serilogView };

            // 执行添加方法
            await _SerilogService.RegisterAsync(serilogView);

            // 消息通知 
            if (base.Notifications.HasErrorNotifications())
                return new MessageModel<object>()
                {
                    Success = false,
                    Message = "Error",
                    //Data = await _OpenCardUserInfoAppService.RetrieveAsync(w => w.CitizenIdNumber == openCardUserInfoView.CitizenIdNumber)
                };
            return new MessageModel<object>()
            {
                Success = true,
                Message = "Ok",
                //Data = await _OpenCardUserInfoAppService.RetrieveAsync(w => w.CitizenIdNumber == openCardUserInfoView.CitizenIdNumber)
            };
        }
        [HttpPost]
        public async Task<MessageModel<object>> GetAllAsync()
        {
            return new MessageModel<object>()
            {
                Success = true,
                Message = "Ok",
                Data = await _SerilogService.GetAllAsync()
            };
        }
    }
}

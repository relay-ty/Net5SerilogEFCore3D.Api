using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Model.DomainCoreModels;

namespace Net5SerilogEFCore3D.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class BaseController<TController> : ControllerBase
    {
        // 将领域通知处理程序注入Controller
        protected readonly DomainNotificationHandler Notifications;
        protected readonly ILogger<TController> Logger;
        //用来进行DTO
        private readonly IMapper Mapper;

        public BaseController(INotificationHandler<DomainNotification> notifications,ILogger<TController> logger, IMapper mapper)
        {
            Notifications = (DomainNotificationHandler)notifications;
            Logger = logger;
            Mapper = mapper;
        }
    }
}

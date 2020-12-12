using MediatR;
using Net5SerilogEFCore3D.Domain.Commands.Serilog;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Infrastructure.Bus;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts;
using Net5SerilogEFCore3D.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.CommandHandlers
{
    /// <summary>
    /// Serilog 命令处理程序
    /// 用来处理该 Serilog 下的所有命令
    /// 注意必须要继承接口IRequestHandler<,>，这样才能实现各个命令的Handle方法
    /// </summary>
    public class SerilogCommandHandler : BaseCommandHandler<SerilogDbContext>,
         IRequestHandler<RegisterSerilogCommand, Unit>
    {
        private readonly ICommandRepository<Serilog, SerilogDbContext> _CommandRepository;
        private readonly IQueryRepository<Serilog, SerilogDbContext> _QueryRepository;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="OpenCardUserInfoRepository"></param>
        /// <param name="uow"></param>
        /// <param name="bus"></param>
        /// <param name="cache"></param>
        public SerilogCommandHandler(
            ICommandRepository<Serilog, SerilogDbContext> commandRepository,  IQueryRepository<Serilog, SerilogDbContext> queryRepository,
            IUnitOfWork<SerilogDbContext> unitOfWork,
            IMediatorHandler bus
            ) : base(unitOfWork, bus)
        {
            _CommandRepository = commandRepository;
            _QueryRepository = queryRepository;
        }


        // RegisterOpenCardUserInfoCommand命令的处理程序
        // 整个命令处理程序的核心都在这里
        // 不仅包括命令验证的收集，持久化，还有领域事件和通知的添加
        public async Task<Unit> Handle(RegisterSerilogCommand message, CancellationToken cancellationToken)
        {
            // 命令验证
            if (!message.IsValid())
            {
                await NotifyValidationErrorsAsync(message); // 错误信息收集               
                return await Task.FromResult(new Unit()); // 返回，结束当前线程
            }
            // 实例化领域模型，这里才真正的用到了领域模型
            // 注意这里是通过构造函数方法实现
            var customer = new Serilog(message.Id, message.Message,message.Level,message.TimeStamp,message.LogEvent,
                message.Remarks, message.IsDeleted, message.IsEnable, message.CreateTime, message.CreateUserId, message.ModifyUserId);

            // 持久化
            await _CommandRepository.AddAsync(customer);
            // 统一提交
            if (await CommitAsync())
            {
                //通过领域事件发布 成功 通知
                await Bus.RaiseEvent(new DomainNotification(HandlerType.Register, NotificationType.Success, "", $"{typeof(Serilog).Name} 登记成功"));
            }
            return await Task.FromResult(new Unit());

        }
    }
    
}

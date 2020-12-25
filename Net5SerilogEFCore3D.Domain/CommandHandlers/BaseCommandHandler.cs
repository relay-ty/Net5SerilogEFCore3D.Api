using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Domain.Core.Commands;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Infrastructure.Bus;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.CommandHandlers
{
    /// <summary>
    /// 领域命令处理程序
    /// 用来作为全部处理程序的基类，提供公共方法和接口数据
    /// </summary>
    public class BaseCommandHandler<TContext> where TContext : DbContext
    {
        // 注入工作单元
        private readonly IUnitOfWork<TContext> _UnitOfWork;
        // 注入中介处理接口=>总线
        protected readonly IMediatorHandler Bus;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="bus"></param>
        /// <param name="cache"></param>
        public BaseCommandHandler(IUnitOfWork<TContext> unitOfWork, IMediatorHandler bus)
        {
            _UnitOfWork = unitOfWork;
            Bus = bus;
        }


        //将领域命令中的验证错误信息收集
        protected void NotifyValidationErrors(GuidCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                //将错误信息提交到事件总线，派发出去
                Bus.RaiseEvent(new DomainNotification(DomainHandlerType.Validation, DomainNotificationType.Error, "", error.ErrorMessage, message.Id));
            }

        }
        //将领域命令中的验证错误信息收集
        protected async Task NotifyValidationErrorsAsync(GuidCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            { //将错误信息提交到事件总线，派发出去
                await Bus.RaiseEvent(new DomainNotification(DomainHandlerType.Validation, DomainNotificationType.Error, "", error.ErrorMessage, message.Id));
            }

        }
        #region  IntCommand
        //将领域命令中的验证错误信息收集
        protected void NotifyValidationErrors(IntCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                //将错误信息提交到事件总线，派发出去
                Bus.RaiseEvent(new DomainNotification(DomainHandlerType.Validation, DomainNotificationType.Error, "", error.ErrorMessage));
            }

        }
        //将领域命令中的验证错误信息收集
        protected async Task NotifyValidationErrorsAsync(IntCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            { //将错误信息提交到事件总线，派发出去
                await Bus.RaiseEvent(new DomainNotification(DomainHandlerType.Validation, DomainNotificationType.Error, "", error.ErrorMessage));
            }

        }
        #endregion 


        //工作单元提交 如果有错误，添加领域通知
        public bool Commit()
        {
            try
            {
                _UnitOfWork.SaveChanges();
                return true;
            }
            catch (Exception error)
            {
                //将错误信息提交到事件总线，派发出去
                Bus.RaiseEvent(new DomainNotification(DomainHandlerType.Commit, DomainNotificationType.Error, "", error.Message));
            }
            return false;
        }
        //工作单元提交 如果有错误，添加领域通知
        public async Task<bool> CommitAsync()
        {
            try
            {
                await _UnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception error)
            {
                //将错误信息提交到事件总线，派发出去
                await Bus.RaiseEvent(new DomainNotification(DomainHandlerType.Commit, DomainNotificationType.Error, "", error.Message));
            }
            return false;
        }
    }
}

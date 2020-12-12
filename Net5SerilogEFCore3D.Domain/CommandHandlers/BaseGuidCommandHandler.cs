using MediatR;
using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Domain.Commands.SDSRHD;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Infrastructure.Bus;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.CommandHandlers
{
    public class BaseGuidCommandHandler<TEntity, TContext> : BaseCommandHandler<TContext>,
        IRequestHandler<RemoveCommand<TEntity>, Unit>,
        IRequestHandler<SoftDeleteCommand<TEntity>, Unit>,
        IRequestHandler<SoftResumeCommand<TEntity>, Unit>

        where TEntity : GuidEntity
        where TContext : DbContext
    {
        // 注入仓储接口
        protected readonly IQueryRepository<TEntity, TContext> QueryRepository;
        protected readonly ICommandRepository<TEntity, TContext> CommandRepository;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="bus"></param>
        /// <param name="cache"></param>
        public BaseGuidCommandHandler(IUnitOfWork<TContext> unitOfWork, IMediatorHandler bus,
            IQueryRepository<TEntity, TContext> queryRepository,
            ICommandRepository<TEntity, TContext> commandRepository) : base(unitOfWork, bus)
        {
            QueryRepository = queryRepository;
            CommandRepository = commandRepository;
        }

        //protected virtual  Guid GetNewId()
        //{
        //    var newId = Guid.NewGuid();
        //    while ( QueryRepository.ExistsByExpression(w => w.Id == newId))
        //        newId = Guid.NewGuid();
        //    return newId;
        //}
        //protected virtual async Task<Guid> GetNewIdAsync()
        //{
        //    var newId = Guid.NewGuid();
        //    while (await QueryRepository.ExistsByExpressionAsync(w => w.Id == newId))
        //        newId = Guid.NewGuid();
        //    return newId;
        //}

        protected virtual bool ExistsByExpression(Expression<Func<TEntity, bool>> expression) => QueryRepository.ExistsByExpression(expression);
        protected virtual TEntity FirstByExpression(Expression<Func<TEntity, bool>> expression) => QueryRepository.FirstByExpression(expression);
        protected virtual async Task<TEntity> FirstByExpressionAsync(Expression<Func<TEntity, bool>> expression) => await QueryRepository.FirstByExpressionAsync(expression);

        protected virtual async Task<TEntity> FindByIdAsync(Guid id) => await FirstByExpressionAsync(w => w.Id == id);//await QueryRepository.FindByIdAsync(id);       
        protected virtual bool ExistsById(Guid id) => FirstByExpression(w => w.Id == id) != null;//QueryRepository.FindById(id) != null;
        protected virtual async Task<bool> ExistsByIdAsync(Guid id) => await FirstByExpressionAsync(w => w.Id == id) != null; //await QueryRepository.FindByIdAsync(id) != null;
        protected virtual async Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression) => await QueryRepository.ExistsByExpressionAsync(expression);


        #region  //删除 软删除 软恢复

        //删除
        public virtual async Task<Unit> Handle(RemoveCommand<TEntity> message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await NotifyValidationErrorsAsync(message);
                return await Task.FromResult(new Unit());
            }

            if (await ExistsByIdAsync(message.Id))
                await CommandRepository.RemoveAsync(message.Id);
            else
            {
                //通过领域事件发布 错误 通知
                await Bus.RaiseEvent(new DomainNotification(HandlerType.Remove, NotificationType.Error, "", $"{typeof(TEntity).Name} 不存在该Id {message.Id}！", message.Id));
                return await Task.FromResult(new Unit());
            }

            if (await CommitAsync())
            {
                //通过领域事件发布 通知 
                await Bus.RaiseEvent(new DomainNotification(HandlerType.Remove, NotificationType.Success, "", $"{typeof(TEntity).Name} 删除成功", message.Id));
            }
            return await Task.FromResult(new Unit());
        }
        //软删除
        public virtual async Task<Unit> Handle(SoftDeleteCommand<TEntity> message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await NotifyValidationErrorsAsync(message);
                return await Task.FromResult(new Unit());
            }
            //注意 软删除 更新 IsDeleted
            var customer = await FindByIdAsync(message.Id);
            if (customer == null)
            {
                //通过领域事件发布 错误 通知
                await Bus.RaiseEvent(new DomainNotification(HandlerType.SoftDelete, NotificationType.Error, "", $"{typeof(TEntity).Name} 不存在该Id {message.Id}！", message.Id));
                return await Task.FromResult(new Unit());
            }
            customer.IsDeleted = true;
            customer.DeletdTime = message.Timestamp;
            await CommandRepository.UpdateAsync(customer);

            if (await CommitAsync())
            {
                //通过领域事件发布 成功 通知
                await Bus.RaiseEvent(new DomainNotification(HandlerType.Remove, NotificationType.Success, "", $"{typeof(TEntity).Name} 软删除成功", message.Id));
            }
            return await Task.FromResult(new Unit());
        }
        //软恢复
        public virtual async Task<Unit> Handle(SoftResumeCommand<TEntity> message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                await NotifyValidationErrorsAsync(message);
                return await Task.FromResult(new Unit());
            }
            //注意 软恢复 更新 IsDeleted
            var customer = await FindByIdAsync(message.Id);
            if (customer == null)
            {
                //通过领域事件发布 错误 通知
                await Bus.RaiseEvent(new DomainNotification(HandlerType.SoftResume, NotificationType.Error, "", $"{typeof(TEntity).Name} 不存在该Id {message.Id}！", message.Id));
                return await Task.FromResult(new Unit());
            }
            customer.IsDeleted = false;
            customer.DeletdTime = null;
            await CommandRepository.UpdateAsync(customer);

            if (await CommitAsync())
            {
                //通过领域事件发布 成功 通知
                await Bus.RaiseEvent(new DomainNotification(HandlerType.Remove, NotificationType.Success, "", $"{typeof(TEntity).Name} 软恢复成功", message.Id));
            }
            return await Task.FromResult(new Unit());
        }
        #endregion


        // 手动回收
        public void Dispose()
        {
            CommandRepository.Dispose();
        }


    }
}

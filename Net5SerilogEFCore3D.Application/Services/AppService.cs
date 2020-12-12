using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Net5SerilogEFCore3D.Application.Interfaces;
using Net5SerilogEFCore3D.Domain.Commands.SDSRHD;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Infrastructure.Bus;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using Net5SerilogEFCore3D.Model.PaginatedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Application.Services
{
    public class AppService<TView, TEntity, TContext> : IAppService<TView, TEntity, TContext>
       where TView : class, new()
       where TEntity : GuidEntity
       where TContext : DbContext
    {
        //注意这里是要IoC依赖注入的
        protected readonly IQueryRepository<TEntity, TContext> QueryRepository;
        //用来进行DTO
        protected readonly IMapper Mapper;
        //中介者 总线
        protected readonly IMediatorHandler Bus;
        // 将领域通知处理程序注入Service 部分业务需要
        protected readonly DomainNotificationHandler Notifications;

        protected readonly ILogger<TEntity> Logger;
        protected readonly IUnitOfWork<TContext> UnitOfWork;

        public AppService(IQueryRepository<TEntity, TContext> queryRepository, IMapper mapper, IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications, ILogger<TEntity> logger, IUnitOfWork<TContext> unitOfWork)
        {
            QueryRepository = queryRepository;
            Mapper = mapper;
            Bus = bus;
            Notifications = (DomainNotificationHandler)notifications;
            Logger = logger;
            UnitOfWork = unitOfWork;
        }



        public virtual bool ExistsById(Guid id) => FirstByExpression(w => w.Id == id) != null; // QueryRepository.FindById(id) != null;

        public virtual async Task<bool> ExistsByIdAsync(Guid id) => await FirstByExpressionAsync(w => w.Id == id) != null; //await QueryRepository.FindByIdAsync(id) != null;

        public virtual bool ExistsByExpression(Expression<Func<TEntity, bool>> expression)
        {
            return QueryRepository.ExistsByExpression(expression);
        }
        public virtual async Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression) =>
            await QueryRepository.ExistsByExpressionAsync(expression);

        public virtual TView FindById(Guid id) => FirstByExpression(w => w.Id == id);// Mapper.Map<TView>(QueryRepository.FindById(id));
        public virtual async Task<TView> FindByIdAsync(Guid id) => await FirstByExpressionAsync(w => w.Id == id);// Mapper.Map<TView>(await QueryRepository.FindByIdAsync(id));
        public virtual TView FirstByExpression(Expression<Func<TEntity, bool>> expression) => Mapper.Map<TView>(QueryRepository.FirstByExpression(expression));

        public virtual async Task<TView> FirstByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Mapper.Map<TView>(await QueryRepository.FirstByExpressionAsync(expression));
        }

        public virtual List<TView> Retrieve(Expression<Func<TEntity, bool>> expression) => Mapper.Map<List<TView>>(QueryRepository.Retrieve(expression));
        public virtual async Task<List<TView>> RetrieveAsync(Expression<Func<TEntity, bool>> expression) => Mapper.Map<List<TView>>(await QueryRepository.RetrieveAsync(expression));
        public virtual async Task<List<TView>> GetAllAsync()
        {
            var getAll = await QueryRepository.GetAllAsync();
            return Mapper.Map<List<TView>>(getAll);
        }




        /// <summary>
        /// 根据 Expression pageIndex pageSize 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<PaginatedList<TView>> PaginatedRetrieve(Expression<Func<TEntity, bool>> expression, int pageIndex = 1, int pageSize = 10)
        {
            var paginatedRetrieve = await QueryRepository.PaginatedRetrieve(expression, pageIndex, pageSize);
            var items = Mapper.Map<List<TView>>(paginatedRetrieve);
            return new PaginatedList<TView>(items, paginatedRetrieve.Total, pageIndex, pageSize);
        }

        public virtual async Task<Guid> GetNewIdAsync()
        {
            var newId = Guid.NewGuid();
            while (await QueryRepository.ExistsByExpressionAsync(w => w.Id == newId))
                newId = Guid.NewGuid();
            return newId;
        }

        #region 删除 软删除 软恢复
        public async Task SoftDeleteAsync(Guid id)
        {
            // 这里引入领域设计中的 软删除命令
            await Bus.SendCommand(new SoftDeleteCommand<TEntity>(id));
            if (!Notifications.GetSpecifyTypeNotifications(NotificationType.Success).Any())
                Logger.LogWarning($"SoftDeleteAsync Erro At {typeof(TEntity).Name}", Notifications.GetNotifications());
        }
        public async Task SoftResumeAsync(Guid id)
        {
            // 这里引入领域设计中的 软恢复命令
            await Bus.SendCommand(new SoftResumeCommand<TEntity>(id));
            if (!Notifications.GetSpecifyTypeNotifications(NotificationType.Success).Any())
                Logger.LogWarning($"SoftResumeAsync Erro At {typeof(TEntity).Name}", Notifications.GetNotifications());
        }

        public async Task RemoveAsync(Guid id)
        {
            // 这里引入领域设计中的 删除命令
            await Bus.SendCommand(new RemoveCommand<TEntity>(id));
            if (!Notifications.GetSpecifyTypeNotifications(NotificationType.Success).Any())
                Logger.LogWarning($"RemoveAsync Erro At {typeof(TEntity).Name}", Notifications.GetNotifications());
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}

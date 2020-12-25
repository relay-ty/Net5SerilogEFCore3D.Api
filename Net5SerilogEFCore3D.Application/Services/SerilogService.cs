using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Net5SerilogEFCore3D.Application.Interfaces;
using Net5SerilogEFCore3D.Domain.Commands.Serilog;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Domain.Core.Notifications;
using Net5SerilogEFCore3D.Infrastructure.Bus;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using Net5SerilogEFCore3D.Model.DomainModels;
using Net5SerilogEFCore3D.Model.PaginatedModels;
using Net5SerilogEFCore3D.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Application.Services
{
    public class SerilogService: ISerilogService
    {
        //注意这里是要IoC依赖注入的
        protected readonly IQueryRepository<Serilog, SerilogDbContext> _QueryRepository;
        //用来进行DTO
        protected readonly IMapper _Mapper;
        //中介者 总线
        protected readonly IMediatorHandler _Bus;
        // 将领域通知处理程序注入Service 部分业务需要
        protected readonly DomainNotificationHandler _Notifications;

        protected readonly ILogger<Serilog> _Logger;
        protected readonly IUnitOfWork<SerilogDbContext> _UnitOfWork;


        public SerilogService(
            IQueryRepository<Serilog, SerilogDbContext> queryRepository,
            IMapper mapper,
            IMediatorHandler bus, INotificationHandler<DomainNotification> notifications,
            ILogger<Serilog> logger, IUnitOfWork<SerilogDbContext> unitOfWork
            ) //: base(citizenIdentityCardInfoQueryRepository, mapper, bus, notifications, logger, unitOfWork)
        {
            _QueryRepository = queryRepository;
            _Mapper = mapper;
            _Bus = bus;
            _Notifications = (DomainNotificationHandler)notifications;
            _Logger = logger;
            _UnitOfWork = unitOfWork;
        }

        public async Task RegisterAsync(SerilogView serilogView)
        {
            // 这里引入领域设计中的写命令
            var registerCommand = _Mapper.Map<RegisterSerilogCommand>(serilogView);
            await _Bus.SendIntCommand(registerCommand);
        }





        public virtual bool ExistsById(int id) => FirstByExpression(w => w.Id == id) != null; // _QueryRepository.FindById(id) != null;

        public virtual async Task<bool> ExistsByIdAsync(int id) => await FirstByExpressionAsync(w => w.Id == id) != null; //await _QueryRepository.FindByIdAsync(id) != null;

        public virtual bool ExistsByExpression(Expression<Func<Serilog, bool>> expression)
        {
            return _QueryRepository.ExistsByExpression(expression);
        }
        public virtual async Task<bool> ExistsByExpressionAsync(Expression<Func<Serilog, bool>> expression) =>
            await _QueryRepository.ExistsByExpressionAsync(expression);

        public virtual SerilogView FindById(int id) => FirstByExpression(w => w.Id == id);// Mapper.Map<SerilogView>(_QueryRepository.FindById(id));
        public virtual async Task<SerilogView> FindByIdAsync(int id) => await FirstByExpressionAsync(w => w.Id == id);// Mapper.Map<SerilogView>(await _QueryRepository.FindByIdAsync(id));
        public virtual SerilogView FirstByExpression(Expression<Func<Serilog, bool>> expression)
        {
            return _Mapper.Map<SerilogView>(_QueryRepository.FirstByExpression(expression));
        }

        public virtual async Task<SerilogView> FirstByExpressionAsync(Expression<Func<Serilog, bool>> expression)
        {
            return _Mapper.Map<SerilogView>(await _QueryRepository.FirstByExpressionAsync(expression));
        }

        public virtual List<SerilogView> Retrieve(Expression<Func<Serilog, bool>> expression) => _Mapper.Map<List<SerilogView>>(_QueryRepository.Retrieve(expression));
        public virtual async Task<List<SerilogView>> RetrieveAsync(Expression<Func<Serilog, bool>> expression) => _Mapper.Map<List<SerilogView>>(await _QueryRepository.RetrieveAsync(expression));
        public virtual async Task<List<SerilogView>> GetAllAsync()
        {
            var getAll = await _QueryRepository.GetAllAsync();
            return _Mapper.Map<List<SerilogView>>(getAll);
        }




        /// <summary>
        /// 根据 Expression pageIndex pageSize 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<PaginatedList<SerilogView>> PaginatedRetrieve(Expression<Func<Serilog, bool>> expression, int pageIndex = 1, int pageSize = 10)
        {
            var paginatedRetrieve = await _QueryRepository.PaginatedRetrieve(expression, pageIndex, pageSize);
            var items = _Mapper.Map<List<SerilogView>>(paginatedRetrieve);
            return new PaginatedList<SerilogView>(items, paginatedRetrieve.Total, pageIndex, pageSize);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

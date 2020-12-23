using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using Net5SerilogEFCore3D.Model.PaginatedModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Application.Interfaces
{
    public interface IAppService<TView, TEntity, TContext> : IDisposable
        where TView : class
        where TEntity : GuidEntity
        where TContext : DbContext
    {
        bool Any();
        Task<bool> AnyAsync();
        bool ExistsById(Guid id);
        Task<bool> ExistsByIdAsync(Guid id);
        bool ExistsByExpression(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        TView FindById(Guid id);
        Task<TView> FindByIdAsync(Guid id);

        TView FirstByExpression(Expression<Func<TEntity, bool>> expression);
        Task<TView> FirstByExpressionAsync(Expression<Func<TEntity, bool>> expression);


        List<TView> Retrieve(Expression<Func<TEntity, bool>> expression);
        Task<List<TView>> RetrieveAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TView>> GetAllAsync();

        /// <summary>
        /// 根据 Expression pageIndex pageSize 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedList<TView>> PaginatedRetrieve(Expression<Func<TEntity, bool>> expression, int pageIndex = 1, int pageSize = 10);

        Task<Guid> GetNewIdAsync();
        #region 删除 软删除 软恢复
        Task SoftDeleteAsync(Guid id);
        Task SoftResumeAsync(Guid id);
        Task RemoveAsync(Guid id);
        #endregion
    }
}

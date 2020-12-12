using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Model.PaginatedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.Core.Interfaces
{
    /// <summary>
    /// 定义泛型仓储接口，并继承IDisposable，显式释放资源
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IQueryRepository<TEntity, TContext> : IDisposable
        where TEntity : class
        where TContext : DbContext
    {
        bool ExistsByExpression(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        //TEntity FindById(Guid id);
        //Task<TEntity> FindByIdAsync(Guid id);
        TEntity FirstByExpression(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FirstByExpressionAsync(Expression<Func<TEntity, bool>> expression);


        List<TEntity> Retrieve(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> RetrieveAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// 根据 Expression pageIndex pageSize 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedList<TEntity>> PaginatedRetrieve(Expression<Func<TEntity, bool>> expression, int pageIndex = 1, int pageSize = 10);
    }
}

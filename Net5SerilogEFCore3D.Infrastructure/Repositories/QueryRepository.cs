using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Model.PaginatedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Infrastructure.Repositories
{
    public class QueryRepository<TEntity, TContext> : IQueryRepository<TEntity, TContext>
       where TEntity : class
       where TContext : DbContext
    {
        protected TContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public QueryRepository(IUnitOfWork<TContext> unitOfWork)
        {
            Context = unitOfWork.GetDbContext();
            DbSet = Context.Set<TEntity>();
        }

        public virtual bool Any() => DbSet.AsNoTracking().Any();
        public virtual async Task<bool> AnyAsync() => await DbSet.AsNoTracking().AnyAsync();

        public virtual bool ExistsByExpression(Expression<Func<TEntity, bool>> expression) => DbSet.AsNoTracking().Any(expression);
        public virtual async Task<bool> ExistsByExpressionAsync(Expression<Func<TEntity, bool>> expression) => await DbSet.AsNoTracking().AnyAsync(expression);
        //public virtual TEntity FindById(Guid id) =>DbSet.Find(id);
        //public virtual async Task<TEntity> FindByIdAsync(Guid id) => await DbSet.FindAsync(id);
        public virtual TEntity FirstByExpression(Expression<Func<TEntity, bool>> expression) => DbSet.AsNoTracking().FirstOrDefault(expression);
        public virtual async Task<TEntity> FirstByExpressionAsync(Expression<Func<TEntity, bool>> expression) => await DbSet.AsNoTracking().FirstOrDefaultAsync(expression);

        public virtual List<TEntity> Retrieve(Expression<Func<TEntity, bool>> expression) => DbSet.AsNoTracking().Where(expression).ToList();
        public virtual async Task<List<TEntity>> RetrieveAsync(Expression<Func<TEntity, bool>> expression) => await DbSet.AsNoTracking().Where(expression).ToListAsync();
        public virtual async Task<List<TEntity>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();


        /// <summary>
        /// 根据 Expression pageIndex pageSize 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<PaginatedList<TEntity>> PaginatedRetrieve(Expression<Func<TEntity, bool>> expression, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 1;
            var source = DbSet.AsNoTracking().Where(expression);
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<TEntity>(items, count, pageIndex, pageSize);//方法用于创建 PaginatedList<T>
        }
        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

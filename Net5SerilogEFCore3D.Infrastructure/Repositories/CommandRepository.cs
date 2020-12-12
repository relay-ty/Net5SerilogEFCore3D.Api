using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Infrastructure.Repositories
{
    /// <summary>
    /// 泛型仓储，实现泛型 仓储接口
    /// </summary>

    public class CommandRepository<TEntity, TContext> : ICommandRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        protected TContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public CommandRepository(IUnitOfWork<TContext> unitOfWork)
        {
            //UnitOfWork = unitOfWork;
            Context = unitOfWork.GetDbContext();
            DbSet = Context.Set<TEntity>();
        }

        #region 添加
        public virtual void Add(TEntity entity) => DbSet.Add(entity);

        public virtual async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

        public virtual void AddRange(List<TEntity> entities) => DbSet.AddRange(entities);

        public virtual async Task AddRangeAsync(List<TEntity> entities) => await DbSet.AddRangeAsync(entities);


        #endregion

        #region 更新
        public virtual void Update(TEntity entity) => DbSet.Update(entity);

        public virtual void UpdateRange(List<TEntity> entities) => DbSet.UpdateRange(entities);

        public virtual async Task UpdateAsync(TEntity entity) => await Task.FromResult(DbSet.Update(entity));
        #endregion

        #region 删除
        public virtual void Remove(Guid id)
        {
            var entity = DbSet.Find(id);
            if (entity != null)
                DbSet.Remove(entity);
        }
        public virtual void Remove(TEntity entity) => DbSet.Remove(entity);
        public virtual void RemoveRange(List<TEntity> entities) => DbSet.RemoveRange(entities);

        public virtual async Task RemoveAsync(Guid id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity != null)
                await Task.FromResult(DbSet.Remove(entity));
        }
        public virtual async Task RemoveAsync(TEntity entity) => await Task.FromResult(DbSet.Remove(entity));

        #endregion

        #region 保存
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
        #endregion

        public virtual void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }


    }
}

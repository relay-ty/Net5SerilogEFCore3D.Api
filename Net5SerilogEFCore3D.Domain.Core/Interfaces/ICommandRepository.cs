using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.Core.Interfaces
{
    /// <summary>
    /// 定义泛型仓储接口，并继承IDisposable，显式释放资源
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICommandRepository<TEntity, TContext> : IDisposable
        where TEntity : class
        where TContext : DbContext
    {
        #region 添加
        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        #endregion

        #region 更新
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);

        Task UpdateAsync(TEntity entity);
        #endregion

        #region 删除
        void Remove(Guid id);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entities);

        Task RemoveAsync(Guid id);
        Task RemoveAsync(TEntity entity);
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}

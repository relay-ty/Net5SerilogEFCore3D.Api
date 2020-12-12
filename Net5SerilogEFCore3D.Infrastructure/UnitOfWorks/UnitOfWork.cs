using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Domain.Core.Interfaces;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Infrastructure.UnitOfWorks
{
    /// <summary>
    /// 工作单元类
    /// </summary>

    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        //数据库上下文
        private readonly TContext _Context;
        //构造函数注入
        public UnitOfWork(TContext context)
        {
            _Context = context;
        }

        public TContext GetDbContext()
        {
            if (_Context != null)
                return _Context;
            else throw new Exception("TContext Is Null");
        }

        //上下文提交       
        public bool SaveChanges()
        {
            if (_Context != null)
            {
                BeforeSave();
                return _Context.SaveChanges() > 0;
            }
            else throw new Exception("TContext Is Null");
        }
        public async Task<bool> SaveChangesAsync()
        {
            if (_Context != null)
            {
                BeforeSave();
                return (await _Context.SaveChangesAsync()) > 0;
            }
            else throw new Exception("TContext Is Null");
        }

        //手动回收
        public void Dispose()
        {
            _Context.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 保存之前
        /// </summary>
        private void BeforeSave()
        {
            foreach (var entry in _Context.ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is GuidEntity)
                        {
                            if (((GuidEntity)entry.Entity).CreateTime == null)
                                ((GuidEntity)entry.Entity).CreateTime = DateTimeOffset.Now;
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is GuidEntity)
                        {
                            ((GuidEntity)entry.Entity).ModifyTime = DateTimeOffset.Now;
                        }
                        break;
                    case EntityState.Deleted://拦截 Deleted 命令 变为 Modified
                        if (entry.Entity is GuidEntity)
                        {
                            var entity = (GuidEntity)entry.Entity;
                            entity.IsDeleted = true;
                            entity.DeletdTime = DateTimeOffset.Now;
                            entry.State = EntityState.Modified;
                        }
                        break;
                }
            }
        }


    }
}

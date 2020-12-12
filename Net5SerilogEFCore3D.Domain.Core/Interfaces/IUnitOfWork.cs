using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.Core.Interfaces
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext GetDbContext();
        Task<bool> SaveChangesAsync();
        //是否提交成功
        bool SaveChanges();
    }
}

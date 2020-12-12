using Net5SerilogEFCore3D.Model.DomainModels;
using Net5SerilogEFCore3D.Model.PaginatedModels;
using Net5SerilogEFCore3D.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Application.Interfaces
{
    /// <summary>
    /// 定义 ISerilogService 服务接口
    /// 并继承IDisposable，显式释放资源
    /// 注意这里我们使用的对象，是视图对象模型
    /// </summary>
    public interface ISerilogService : IDisposable
    {
        Task RegisterAsync(SerilogView serilogView);





        bool ExistsById(int id);

        Task<bool> ExistsByIdAsync(int id);
        bool ExistsByExpression(Expression<Func<Serilog, bool>> expression);
        Task<bool> ExistsByExpressionAsync(Expression<Func<Serilog, bool>> expression);
        SerilogView FindById(int id);
        Task<SerilogView> FindByIdAsync(int id);
        SerilogView FirstByExpression(Expression<Func<Serilog, bool>> expression);

        Task<SerilogView> FirstByExpressionAsync(Expression<Func<Serilog, bool>> expression);
        List<SerilogView> Retrieve(Expression<Func<Serilog, bool>> expression);
        Task<List<SerilogView>> RetrieveAsync(Expression<Func<Serilog, bool>> expression);
        Task<List<SerilogView>> GetAllAsync();




        /// <summary>
        /// 根据 Expression pageIndex pageSize 分页查询
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginatedList<SerilogView>> PaginatedRetrieve(Expression<Func<Serilog, bool>> expression, int pageIndex = 1, int pageSize = 10);
    }
}

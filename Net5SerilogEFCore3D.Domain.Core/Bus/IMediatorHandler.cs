using Net5SerilogEFCore3D.Domain.Core.Commands;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Infrastructure.Bus
{
    /// <summary>
    /// 中介处理程序接口
    /// 可以定义多个处理程序
    /// 是异步的
    /// </summary>
    public interface IMediatorHandler
    {
        /// <summary>
        /// 发送命令，将我们的命令模型发布到中介者模块
        /// </summary>
        /// <typeparam name="T"> 泛型 </typeparam>
        /// <param name="command"> 命令模型，比如 RegisterCitizenIdentityCardInfoCommand </param>
        /// <returns></returns>
        Task SendCommand<T>(T command) where T : GuidCommand;

         /// <summary>
        /// 发送命令，将我们的命令模型发布到中介者模块
        /// </summary>
        /// <typeparam name="T"> 泛型 </typeparam>
        /// <param name="command"> 命令模型，比如 RegisterCitizenIdentityCardInfoCommand </param>
        /// <returns></returns>
        Task SendIntCommand<T>(T command) where T : IntCommand;


        /// <summary>
        /// 引发事件，通过总线，发布事件
        /// </summary>
        /// <typeparam name="T"> 泛型 继承 Event：INotification</typeparam>
        /// <param name="event"> 事件模型，比如CitizenIdentityCardInfoRegisteredEvent，</param>
        /// 请注意一个细节：这个命名方法和Command不一样，一个是 RegisterCitizenIdentityCardInfoCommand 注册命令之前,一个是CitizenIdentityCardInfoRegisteredEvent 被注册事件之后
        /// <returns></returns>
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}

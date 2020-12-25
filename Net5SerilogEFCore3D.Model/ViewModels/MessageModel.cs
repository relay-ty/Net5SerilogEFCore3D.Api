using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System.Collections.Generic;

namespace Net5SerilogEFCore3D.Model.ViewModels
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel<TData>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = false;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public TData Data { get; set; }

        public List<DomainNotification> Notifications { get; set; }
    }
}

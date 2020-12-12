using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Model.ViewModels
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel<T>
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
        public T Data { get; set; }

    }
}

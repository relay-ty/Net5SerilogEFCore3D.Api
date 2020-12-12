using FluentValidation.Results;
using MediatR;
using System;

namespace Net5SerilogEFCore3D.Domain.Core.Commands
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class IntCommand : IRequest
    {
        //公共参数主键Id
        public int Id { get; protected set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; protected set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDeleted { get; set; } = false;
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTimeOffset? DeletdTime { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool? IsEnable { get; protected set; } = true;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset? CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>        
        public Guid? CreateUserId { get; protected set; }

        /// <summary> 
        /// 修改时间
        /// </summary>
        public DateTimeOffset? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public Guid? ModifyUserId { get; protected set; }




        //时间戳
        public DateTimeOffset Timestamp { get; private set; }
        //验证结果，需要引用FluentValidation
        public ValidationResult ValidationResult { get; set; }

        protected IntCommand()
        {
            Timestamp = DateTimeOffset.Now;
        }

        //定义抽象方法，是否有效
        public abstract bool IsValid();
    }
}

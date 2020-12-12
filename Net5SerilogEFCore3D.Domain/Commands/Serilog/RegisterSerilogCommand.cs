using Net5SerilogEFCore3D.Domain.Validations.Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.Commands.Serilog
{
    /// <summary>
    /// 注册一个添加  命令
    /// 基础抽象 命令模型
    /// </summary>
    public class RegisterSerilogCommand : SerilogCommand
    {
        // set 受保护，只能通过构造函数方法赋值
        public RegisterSerilogCommand(int id, string message, string level, DateTimeOffset timeStamp, string logEvent
            , string remarks, bool? isEnable, DateTimeOffset? createTime, Guid? createUserId)
        {
            Id = id;
            Message = message;
            Level = level;
            TimeStamp = timeStamp;
            LogEvent = logEvent;

            Remarks = remarks;
            IsEnable = isEnable;
            CreateTime = createTime;
            CreateUserId = createUserId;
        }

        // 重写基类中的 是否有效 方法
        // 主要是为了引入命令验证 RegisterSerilogCommandValidation。
        public override bool IsValid()
        {
            ValidationResult = new RegisterSerilogCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

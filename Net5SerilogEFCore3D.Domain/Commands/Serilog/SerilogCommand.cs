using Net5SerilogEFCore3D.Domain.Core.Commands;
using System;

namespace Net5SerilogEFCore3D.Domain.Commands.Serilog
{

    /// <summary>
    /// 定义一个抽象的 Serilog 命令模型
    /// 继承 Command
    /// 这个模型主要作用就是用来创建命令动作的，所以是一个抽象类
    /// </summary>
    public abstract class SerilogCommand : IntCommand
    {
        public string Message { get; protected set; }
        public string Level { get; protected set; }
        public DateTimeOffset TimeStamp { get; protected set; }
        public string Exception { get; protected set; }
        public string LogEvent { get; protected set; }
    }
}

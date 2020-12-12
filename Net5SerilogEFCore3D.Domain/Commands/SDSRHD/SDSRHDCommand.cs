using Net5SerilogEFCore3D.Domain.Core.Commands;

namespace Net5SerilogEFCore3D.Domain.Commands.SDSRHD
{
    /// <summary>
    /// 定义一个抽象的  命令模型
    /// 继承 Command
    /// 这个模型主要作用就是用来创建命令动作的，所以是一个抽象类
    /// </summary>

    public abstract class SDSRHDCommand:GuidCommand
    {
    }
}

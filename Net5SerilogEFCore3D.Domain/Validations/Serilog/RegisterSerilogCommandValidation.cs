using Net5SerilogEFCore3D.Domain.Commands.Serilog;

namespace Net5SerilogEFCore3D.Domain.Validations.Serilog
{
    /// <summary>
    /// 添加 Serilog 命令模型验证
    /// 继承 SerilogValidation 基类
    /// </summary>
    public class RegisterSerilogCommandValidation : SerilogValidation<RegisterSerilogCommand>
    {
        public RegisterSerilogCommandValidation()
        {
            ValidateId();
            //可以自定义增加新的验证
        }
    }
}

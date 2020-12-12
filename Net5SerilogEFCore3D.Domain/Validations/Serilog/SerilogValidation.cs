using FluentValidation;
using Net5SerilogEFCore3D.Domain.Commands.Serilog;
using System;

namespace Net5SerilogEFCore3D.Domain.Validations.Serilog
{

    /// <summary>
    /// 定义基于  Command 的抽象基类  Validation
    /// 继承 抽象类 AbstractValidator
    /// 注意需要引用 FluentValidation
    /// 注意这里的 T 是命令模型
    /// </summary>
    /// <typeparam name="T">泛型类</typeparam>
    public abstract class SerilogValidation<TCommand> : AbstractValidator<TCommand> where TCommand : SerilogCommand
    {

        //验证Guid
        protected void ValidateId()
        {
            RuleFor(c => c.Id).NotNull();
        }
    }
}

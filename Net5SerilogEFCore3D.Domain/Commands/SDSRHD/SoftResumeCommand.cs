using Net5SerilogEFCore3D.Domain.Validations.SDSRHD;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System;

namespace Net5SerilogEFCore3D.Domain.Commands.SDSRHD
{
    /// <summary>
    /// 注册一个 软恢复  命令
    /// 基础抽象 命令模型
    /// </summary>
    public class SoftResumeCommand<TEntity> : SDSRHDCommand
        where TEntity : GuidEntity
    {
        public SoftResumeCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new SoftResumeCommandValidation<TEntity>().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

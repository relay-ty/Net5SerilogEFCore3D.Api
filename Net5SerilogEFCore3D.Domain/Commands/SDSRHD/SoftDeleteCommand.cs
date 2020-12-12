using Net5SerilogEFCore3D.Domain.Validations.SDSRHD;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System;

namespace Net5SerilogEFCore3D.Domain.Commands.SDSRHD
{
    /// <summary>
    /// 注册一个 软删除  命令
    /// 基础抽象 命令模型
    /// </summary>
    public class SoftDeleteCommand<TEntity> : SDSRHDCommand
        where TEntity : GuidEntity
    {
        public SoftDeleteCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new SoftDeleteCommandValidation<TEntity>().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

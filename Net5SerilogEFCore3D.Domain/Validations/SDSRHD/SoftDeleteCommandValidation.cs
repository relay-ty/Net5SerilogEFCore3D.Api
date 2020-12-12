using Net5SerilogEFCore3D.Domain.Commands.SDSRHD;
using Net5SerilogEFCore3D.Model.DomainCoreModels;

namespace Net5SerilogEFCore3D.Domain.Validations.SDSRHD
{
    public class SoftDeleteCommandValidation<TEntity> : SDSRHDValidation<SoftDeleteCommand<TEntity>>
          where TEntity : GuidEntity
    {
        public SoftDeleteCommandValidation()
        {
            ValidateId();
        }
    }
}

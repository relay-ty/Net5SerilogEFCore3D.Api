using Net5SerilogEFCore3D.Domain.Commands.SDSRHD;
using Net5SerilogEFCore3D.Model.DomainCoreModels;

namespace Net5SerilogEFCore3D.Domain.Validations.SDSRHD
{
    public class SoftResumeCommandValidation<TEntity> : SDSRHDValidation<SoftResumeCommand<TEntity>>
          where TEntity : GuidEntity
    {
        public SoftResumeCommandValidation()
        {
            ValidateId();
        }
    }
}

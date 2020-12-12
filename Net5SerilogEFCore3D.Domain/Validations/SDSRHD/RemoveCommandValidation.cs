using Net5SerilogEFCore3D.Domain.Commands.SDSRHD;
using Net5SerilogEFCore3D.Model.DomainCoreModels;

namespace Net5SerilogEFCore3D.Domain.Validations.SDSRHD
{
    public class RemoveCommandValidation<TEntity> : SDSRHDValidation<RemoveCommand<TEntity>>
          where TEntity : GuidEntity
    {
        public RemoveCommandValidation()
        {
            ValidateId();
        }
    }
}

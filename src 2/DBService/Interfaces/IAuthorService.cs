using DBService.Entities;
using DBService.Interfaces.CommonInterfaces;


namespace DBService.Interfaces
{
    public interface IAuthorService: IValidateUniqueText, IBasicOperations<Author>, IModifyOperations<Author>
    {
    }

}

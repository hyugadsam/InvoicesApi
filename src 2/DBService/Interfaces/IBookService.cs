using DBService.Entities;
using DBService.Interfaces.CommonInterfaces;
using Dtos.Request;

namespace DBService.Interfaces
{
    public interface IBookService : IValidateUniqueText, IBasicOperations<Book>, IModifyOperations<UpdateBookRequest>
    {
    }

}

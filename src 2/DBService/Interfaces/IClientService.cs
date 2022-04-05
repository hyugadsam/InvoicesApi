using DBService.Entities;
using DBService.Interfaces.CommonInterfaces;
using Dtos.Request;

namespace DBService.Interfaces
{
    public interface IClientService : IValidateUniqueText, IBasicOperations<Client>, IModifyOperations<UpdateClientRequest>
    {
    }

}

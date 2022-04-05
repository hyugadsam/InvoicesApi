using Dtos.Common;
using WebSite.Interfaces.Common;
using WebSite.Models.Clients;

namespace WebSite.Interfaces
{
    public interface IClientsAppService : IGetList<ClientDto>, IGetItem<FullClientDto>, IModifyOperations<UpdateClientModel>, 
                                        ICreate<NewClientModel>, ICrudModels<UpdateClientModel, string>
    {
    }
}

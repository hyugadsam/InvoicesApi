using DBService.Entities;
using Dtos.Request;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IClientService: IValidateUniqueText
    {
        Task<BasicCreateResponse> CreateClient(Client client);
        Task<List<Client>> GetClientList();
        Task<Client> GetClient(int Clientid);
        Task<BasicResponse> UpdateClient(UpdateClientRequest request);
        Task<BasicResponse> DeleteClient(int Clientid);

    }

}

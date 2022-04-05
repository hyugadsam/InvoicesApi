using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceClients
    {
        private readonly IMapper mapper;
        private readonly IClientService service;

        public AppServiceClients(IClientService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        public async Task<BasicCreateResponse> CreateClient(string Name)
        {
            if (await service.CheckUniqueEntityText(Name))
                return new BasicCreateResponse { Code = 400, Message = "Ya existe un cliente con el mismo nombre" };

            var client = new Client { Name = Name };
            return await service.Create(client);
        }

        public async Task<List<ClientDto>> GetClientList(PaginationDto pagination)
        {
            var clients = await service.GetList(pagination);
            return mapper.Map<List<ClientDto>>(clients);
        }

        public async Task<FullClientDto> GetClient(int Clientid)
        {
            var client = await service.Get(Clientid);
            return mapper.Map<FullClientDto>(client);
        }

        public async Task<BasicResponse> UpdateClient(UpdateClientRequest request)
        {
            return await service.Update(request);
        }

        public async Task<BasicResponse> DeleteClient(int Clientid)
        {
            return await service.Delete(Clientid);
        }



    }

}

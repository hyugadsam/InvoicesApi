using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
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
            return await service.CreateClient(client);
        }

        public async Task<List<ClientDto>> GetClientList()
        {
            var clients = await service.GetClientList();
            return mapper.Map<List<ClientDto>>(clients);
        }

        public async Task<FullClientDto> GetClient(int Clientid)
        {
            var client = await service.GetClient(Clientid);
            return mapper.Map<FullClientDto>(client);
        }

        public async Task<BasicResponse> UpdateClient(UpdateClientRequest request)
        {
            return await service.UpdateClient(request);
        }

        public async Task<BasicResponse> DeleteClient(int Clientid)
        {
            return await service.DeleteClient(Clientid);
        }



    }

}

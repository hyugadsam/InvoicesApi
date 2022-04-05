using ApplicationServices.Services;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace InvoicesWebApp.Controllers
{
    [Route("api/Clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppServiceClients service;

        public ClientsController(AppServiceClients service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<BasicCreateResponse> CreateClient([FromBody] string Name)
        {
            return await service.CreateClient(Name);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<ClientDto>> GetClientList([FromQuery] PaginationDto pagination)
        {
            return await service.GetClientList(pagination);
        }


        [HttpGet]
        [Route("Get/{Clientid:int}")]
        public async Task<FullClientDto> GetClient(int Clientid)
        {
            return await service.GetClient(Clientid);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<BasicResponse> UpdateClient(UpdateClientRequest request)
        {
            return await service.UpdateClient(request);
        }

        [HttpDelete]
        [Route("Delete/{Clientid:int}")]
        public async Task<BasicResponse> DeleteClient(int Clientid)
        {
            return await service.DeleteClient(Clientid);
        }




    }
}

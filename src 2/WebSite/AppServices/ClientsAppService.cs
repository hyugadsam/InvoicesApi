using AutoMapper;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Interfaces;
using WebSite.Models.Books;
using WebSite.Models.Clients;
using WebSite.Services;

namespace WebSite.AppServices
{
    public class ClientsAppService : IClientsAppService
    {
        private readonly ILogger<ClientsAppService> logger;
        private readonly IWebApiService service;
        private readonly IMapper mapper;
        private readonly IBaseAppService baseService;

        public ClientsAppService(ILogger<ClientsAppService> logger, IWebApiService service, IMapper mapper, IBaseAppService baseService)
        {
            this.logger = logger;
            this.service = service;
            this.mapper = mapper;
            this.baseService = baseService;
        }

        public async Task<BasicCreateResponse> Create(NewClientModel obj)
        {
            try
            {
                return await service.CreateClient(obj.Name);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.ClientsAppService.Create", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error ClientsAppService.Create: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

        public async Task<BasicResponse> Delete(int Id)
        {
            try
            {
                return await service.DeleteClient(Id);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.ClientsAppService.Delete", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error ClientsAppService.Delete: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

        public async Task<FullClientDto> Get(int Id)
        {
            try
            {
                return await service.GetClient(Id);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.ClientsAppService.Get", ex);
                return null;
            }
        }

        public async Task<List<ClientDto>> GetList(Models.Common.PaginationDto pagination)
        {
            try
            {
                return await service.GetClientList(pagination);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.ClientsAppService.GetList", ex);
                return new List<ClientDto>();
            }
        }

        public async Task<UpdateClientModel> GetUpdateModel(int Id)
        {
            try
            {
                var client = await service.GetClient(Id);
                if (client == null || client?.ClientId != Id)
                    return null;

                var books = await service.GetBookList(baseService.GetFullList());

                var model = new UpdateClientModel() { ClientId = client.ClientId, Name = client.Name };
                model.MarkSelectedBooks(books, client.BorrowedBooks);

                return model;
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.ClientsAppService.GetUpdateModel", ex);
                return null;
            }
        }
        
        public async Task<BasicResponse> Update(UpdateClientModel obj)
        {
            try
            {
                var request = new UpdateClientRequest
                {
                    BorrowedBooks = obj.BooksId,
                    ClientId = obj.ClientId,
                    Name = obj.Name
                };

                return await service.UpdateClient(request);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.ClientsAppService.Update", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error ClientsAppService.Update: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

    }
}

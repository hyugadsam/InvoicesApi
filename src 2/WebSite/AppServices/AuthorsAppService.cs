using AutoMapper;
using Dtos.Common;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Interfaces;
using WebSite.Models.Authors;
using WebSite.Services;

namespace WebSite.AppServices
{
    public class AuthorsAppService : IAuthorsAppService
    {
        private readonly IWebApiService service;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsAppService> logger;

        public AuthorsAppService(IWebApiService service, IMapper mapper, ILogger<AuthorsAppService> logger)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BasicCreateResponse> Create(NewAuthorModel obj)
        {
            try
            {
                return await service.CreateAuthor(obj.Name);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.AuthorsAppService.Create", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error AuthorsAppService.Create: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

        public async Task<BasicResponse> Delete(int Id)
        {
            try
            {
                return await service.DeleteAuthor(Id);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.AuthorsAppService.Delete", ex);
                return new BasicResponse { Code = 500, Message = $"Error AuthorsAppService.Delete: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

        public async Task<FullAuthorDto> Get(int Id)
        {
            try
            {
                return await service.GetAuthor(Id);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.AuthorsAppService.Get", ex);
                return null;
            }
        }

        public async Task<List<AuthorDto>> GetList(Models.Common.PaginationDto pagination)
        {
            try
            {
                return await service.GetAuthorList(pagination);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.AuthorsAppService.GetList", ex);
                return new List<AuthorDto>();
            }
        }

        public async Task<BasicResponse> Update(AuthorDto obj)
        {
            try
            {
                return await service.UpdateAuthor(obj);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.AuthorsAppService.Update", ex);
                return new BasicResponse { Code = 500, Message = $"Error AuthorsAppService.UpdateAuthor: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

    }
}

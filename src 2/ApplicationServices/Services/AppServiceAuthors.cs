using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using Dtos.Common;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceAuthors
    {
        private readonly IMapper mapper;
        private readonly ILogger<AppServiceAuthors> logger;
        private readonly IAuthorService service;

        public AppServiceAuthors(IAuthorService service, IMapper mapper, ILogger<AppServiceAuthors> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.service = service;
        }

        public async Task<BasicCreateResponse> CreateAuthor(string Name)
        {
            try
            {
                if (await service.CheckUniqueEntityText(Name))
                    return new BasicCreateResponse { Code = 400, Message = "Ya existe un autor con el mismo nombre" };

                var author = new Author { Name = Name };
                return await service.Create(author);
            }
            catch (Exception ex)
            {
                logger.LogError("Error en ApplicationServices.Services.AppServiceAuthors CreateAuthor", ex);
                return new BasicCreateResponse { Code= 500, Message = $"Error: {ex.Message} -- {ex.InnerException?.Message}" };
            }
            
        }

        public async Task<List<AuthorDto>> GetAuthorList(PaginationDto pagination)
        {
            try
            {
                var authors = await service.GetList(pagination);
                return mapper.Map<List<AuthorDto>>(authors);
            }
            catch (Exception ex)
            {
                logger.LogError("Error en ApplicationServices.Services.AppServiceAuthors GetAuthorList", ex);
                return new List<AuthorDto>();
            }
        }

        public async Task<FullAuthorDto> GetAuthor(int Authorid)
        {
            try
            {
                var item = await service.Get(Authorid);
                return mapper.Map<FullAuthorDto>(item);
            }
            catch (Exception ex)
            {
                logger.LogError("Error en ApplicationServices.Services.AppServiceAuthors GetAuthor", ex);
                return new FullAuthorDto();
            }
            
        }

        public async Task<BasicResponse> UpdateAuthor(AuthorDto request)
        {
            try
            {
                var author = mapper.Map<Author>(request);
                return await service.Update(author);
            }
            catch (Exception ex)
            {
                logger.LogError("Error en ApplicationServices.Services.AppServiceAuthors UpdateAuthor", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error: {ex.Message} -- {ex.InnerException?.Message}" };
            }
            
        }

        public async Task<BasicResponse> DeleteAuthor(int AuthorId)
        {
            try
            {
                return await service.Delete(AuthorId);
            }
            catch (Exception ex)
            {
                logger.LogError("Error en ApplicationServices.Services.AppServiceAuthors DeleteAuthor", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error: {ex.Message} -- {ex.InnerException?.Message}" };
            }
        }


    }
}

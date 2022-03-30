using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Common;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceAuthors
    {
        private readonly IMapper mapper;
        private readonly IAuthorService service;

        public AppServiceAuthors(IAuthorService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        public async Task<BasicCreateResponse> CreateAuthor(string Name)
        {
            if (await service.CheckUniqueEntityText(Name))
                return new BasicCreateResponse { Code = 400, Message = "Ya existe un autor con el mismo nombre" };

            var author = new Author { Name = Name };
            return await service.CreateAuthor(author);
        }

        public async Task<List<AuthorDto>> GetAuthorList()
        {
            var authors = await service.GetAuthorList();
            return mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<FullAuthorDto> GetAuthor(int Authorid)
        {
            var item = await service.GetAuthor(Authorid);
            return mapper.Map<FullAuthorDto>(item);
        }

        public async Task<BasicResponse> UpdateAuthor(AuthorDto request)
        {
            var author = mapper.Map<Author>(request);
            return await service.UpdateAuthor(author);
        }

        public async Task<BasicResponse> DeleteAuthor(int AuthorId)
        {
            return await service.DeleteAuthor(AuthorId);
        }


    }
}

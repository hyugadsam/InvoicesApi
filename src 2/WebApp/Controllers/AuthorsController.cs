using ApplicationServices.Services;
using Dtos.Common;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace InvoicesWebApp.Controllers
{
    [Route("api/Authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppServiceAuthors service;

        public AuthorsController(AppServiceAuthors service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<BasicCreateResponse> CreateAuthor([FromBody] string Name)
        {
            return await service.CreateAuthor(Name);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<AuthorDto>> GetAuthorList([FromQuery] PaginationDto pagination)
        {
            return await service.GetAuthorList(pagination);
        }

        [HttpGet]
        [Route("Get/{Authorid:int}")]
        public async Task<FullAuthorDto> GetAuthor(int Authorid)
        {
            return await service.GetAuthor(Authorid);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<BasicResponse> UpdateAuthor(AuthorDto request)
        {
            return await service.UpdateAuthor(request);
        }

        [HttpDelete]
        [Route("Delete/{AuthorId:int}")]
        public async Task<BasicResponse> DeleteAuthor(int AuthorId)
        {
            return await service.DeleteAuthor(AuthorId);
        }



    }
}

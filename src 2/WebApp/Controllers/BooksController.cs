using ApplicationServices.Services;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoicesWebApp.Controllers
{
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppServiceBooks service;

        public BooksController(AppServiceBooks service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<BasicCreateResponse> CreateBook([FromBody] string Title)
        {
            return await service.CreateBook(Title);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<BookDto>> GetBookList()
        {
            return await service.GetBookList();
        }

        [HttpGet]
        [Route("Get/{BookId:int}")]
        public async Task<FullBookDto> GetBook(int BookId)
        {
            return await service.GetBook(BookId);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<BasicResponse> UpdateBook(UpdateBookRequest request)
        {
            return await service.UpdateBook(request);
        }

        [HttpDelete]
        [Route("Delete/{BookId:int}")]
        public async Task<BasicResponse> DeleteBook(int BookId)
        {
            return await service.DeleteBook(BookId);
        }




    }
}

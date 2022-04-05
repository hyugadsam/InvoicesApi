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
    public class AppServiceBooks
    {
        private readonly IMapper mapper;
        private readonly IBookService service;

        public AppServiceBooks(IBookService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        public async Task<BasicCreateResponse> CreateBook(string Title)
        {
            if (await service.CheckUniqueEntityText(Title))
                return new BasicCreateResponse { Code = 400, Message = "Ya existe un libro con el mismo titulo" };

            var book = new Book { Title = Title };
            return await service.Create(book);
        }

        public async Task<List<BookDto>> GetBookList(PaginationDto pagination)
        {
            var books = await service.GetList(pagination);
            return mapper.Map<List<BookDto>>(books);
        }

        public async Task<FullBookDto> GetBook(int BookId)
        {
            var book = await service.Get(BookId);
            return mapper.Map<FullBookDto>(book);
        }

        public async Task<BasicResponse> UpdateBook(UpdateBookRequest request)
        {
            return await service.Update(request);
        }

        public async Task<BasicResponse> DeleteBook(int BookId)
        {
            return await service.Delete(BookId);
        }


    }
}

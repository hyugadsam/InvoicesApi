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
            return await service.CreateBook(book);
        }

        public async Task<List<BookDto>> GetBookList()
        {
            var books = await service.GetBookList();
            return mapper.Map<List<BookDto>>(books);
        }

        public async Task<FullBookDto> GetBook(int BookId)
        {
            var book = await service.GetBook(BookId);
            return mapper.Map<FullBookDto>(book);
        }

        public async Task<BasicResponse> UpdateBook(UpdateBookRequest request)
        {
            return await service.UpdateBook(request);
        }

        public async Task<BasicResponse> DeleteBook(int BookId)
        {
            return await service.DeleteBook(BookId);
        }

    }


}

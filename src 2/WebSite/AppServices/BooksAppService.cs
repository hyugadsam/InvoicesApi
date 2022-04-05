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
using WebSite.Models.Authors;
using WebSite.Models.Books;
using WebSite.Services;

namespace WebSite.AppServices
{
    public class BooksAppService : IBooksAppService
    {
        private readonly IWebApiService service;
        private readonly IMapper mapper;
        private readonly ILogger<BooksAppService> logger;
        private readonly IBaseAppService baseService;

        public BooksAppService(IWebApiService service, IMapper mapper, ILogger<BooksAppService> logger, IBaseAppService baseService)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
            this.baseService = baseService;
        }
        public async Task<BasicCreateResponse> Create(NewBookModel obj)
        {
            try
            {
                return await service.CreateBook(obj.Title);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.BooksAppService.Create", ex);
                return new BasicCreateResponse { Code = 500, Message = $"Error BooksAppService.Create: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

        public async Task<BasicResponse> Delete(int Id)
        {
            try
            {
                return await service.DeleteBook(Id);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.BooksAppService.Create", ex);
                return new BasicResponse { Code = 500, Message = $"Error BooksAppService.Delete: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }

        public async Task<FullBookDto> Get(int Id)
        {
            try
            {
                return await service.GetBook(Id);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.BooksAppService.Get", ex);
                return null;
            }
        }

        public async Task<List<BookDto>> GetList(Models.Common.PaginationDto pagination)
        {
            try
            {
                return await service.GetBookList(pagination);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.BooksAppService.GetList", ex);
                return new List<BookDto>();
            }
        }

        public async Task<UpdateBookModel> GetUpdateModel(int Id)
        {
            try
            {
                var book = await Get(Id);
                if (book == null || book?.BookId != Id)
                {
                    return null;
                }

                var authors = await service.GetAuthorList(baseService.GetFullList());
                var model = mapper.Map<UpdateBookModel>(book);
                model.AuthorList = mapper.Map<List<SelectAuthorModel>>(authors);
                model.AuthorList.ForEach( a => a.Check = book.AuthorsList.Any(ba => ba.AuthorId == a.Author.AuthorId));    //Marcar los asignados

                return model;
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.BooksAppService.GetUpdateModel", ex);
                return null;
            }
        }

        public async Task<BasicResponse> Update(UpdateBookModel obj)
        {
            try
            {
                var request = mapper.Map<UpdateBookRequest>(obj);
                return await service.UpdateBook(request);
            }
            catch (Exception ex)
            {
                logger.LogError("WebSite.AppServices.BooksAppService.Update", ex);
                return new BasicResponse { Code = 500, Message = $"Error BooksAppService.Update: {ex.Message} \n {ex.InnerException?.Message}" };
            }
        }


    }
}

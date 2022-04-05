using DBService.Entities;
using DBService.Interfaces;
using DBService.Utils;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBService.Services
{
    public class BookService : IBookService
    {
        private readonly DataBaseContext context;
        private readonly ILogger<BookService> logger;

        public BookService(DataBaseContext context, ILogger<BookService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicCreateResponse> Create(Book obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                context.Books.Add(obj);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = obj.BookId
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.BookService.CreateBook");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<Book>> GetList(PaginationDto pagination)
        {
            try
            {
                return await context.Books.GetRecordsByPages(pagination).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.BookService.GetBookList");
                return null;
            }
        }

        public async Task<Book> Get(int Id)
        {
            return await GetBookInfo(Id, true);
        }

        public async Task<BasicResponse> Update(UpdateBookRequest obj)
        {
            try
            {
                var dbBook = await GetBookInfo(obj.BookId);
                if (dbBook == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El libro {obj.BookId} no existe en la bd"
                    };
                }
                dbBook.Title = obj.Title;
                dbBook.CreateDate = obj.CreateDate;
                dbBook.AuthorBooks = obj.AuthorsList.Select( a => new AuthorBook { BookId = obj.BookId, AuthorId = a } ).ToList();

                context.Entry(dbBook).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = "update Success"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.BookService.CreateBook");
                return new BasicResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BasicResponse> Delete(int Id)
        {
            try
            {
                var dbBook = await GetBookInfo(Id);
                if (dbBook == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El libro {Id} no existe en la bd"
                    };
                }
                
                context.Books.Remove(dbBook);

                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = "Delete Success"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.BookService.DeleteBook");
                return new BasicResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        private async Task<Book> GetBookInfo(int Bookid, bool addAutors = false)
        {
            try
            {
                var query = context.Books.Where(b => b.BookId == Bookid);

                if (addAutors)
                    query = query.Include(book => book.AuthorBooks).ThenInclude(a => a.Author);
                else
                    query = query.Include(book => book.AuthorBooks);


                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.BookService.GetBookInfo");
                return null;
            }

        }

        public async Task<bool> CheckUniqueEntityText(string text)
        {
            return await context.Books.AnyAsync(a => a.Title.ToLower().Equals(text.ToLower()));
        }

    }
}

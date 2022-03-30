using DBService.Entities;
using DBService.Interfaces;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<BasicCreateResponse> CreateBook(Book book)
        {
            try
            {
                book.CreateDate = DateTime.Now;
                context.Books.Add(book);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = book.BookId
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

        public async Task<List<Book>> GetBookList()
        {
            try
            {
                return await context.Books.Include(b => b.AuthorBooks).ThenInclude(a => a.Author).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.BookService.GetBookList");
                return null;
            }
        }

        public async Task<Book> GetBook(int BookId)
        {
            return await GetBookInfo(BookId, true);
        }

        public async Task<BasicResponse> UpdateBook(UpdateBookRequest request)
        {
            try
            {
                var dbBook = await GetBookInfo(request.BookId);
                if (dbBook == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El libro {request.BookId} no existe en la bd"
                    };
                }
                dbBook.Title = request.Title;
                dbBook.CreateDate = request.CreateDate;
                dbBook.AuthorBooks = request.AuthorsList.Select( a => new AuthorBook { BookId = request.BookId, AuthorId = a } ).ToList();

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

        public async Task<BasicResponse> DeleteBook(int BookId)
        {
            try
            {
                var dbBook = await GetBookInfo(BookId);
                if (dbBook == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El libro {BookId} no existe en la bd"
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
                return await context.Books.Where(b => b.BookId == Bookid).Include(book => book.AuthorBooks).ThenInclude(a => a.Author).FirstOrDefaultAsync();
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

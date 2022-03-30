using DBService.Entities;
using DBService.Interfaces;
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
    public class AuthorService : IAuthorService
    {
        private readonly DataBaseContext context;
        private readonly ILogger<AuthorService> logger;

        public AuthorService(DataBaseContext context, ILogger<AuthorService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicCreateResponse> CreateAuthor(Author author)
        {
            try
            {
                context.Authors.Add(author);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = author.AuthorId
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.CreateAuthor");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<Author>> GetAuthorList()
        {
            try
            {
                return await context.Authors.Include(ab => ab.AuthorBooks).ThenInclude(b => b.Book).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.GetAuthorList");
                return null;
            }
        }

        public async Task<Author> GetAuthor(int Authorid)
        {
            return await context.Authors.Where(a => a.AuthorId == Authorid).Include(b => b.AuthorBooks).ThenInclude(bk => bk.Book).FirstOrDefaultAsync();
        }

        public async Task<BasicResponse> UpdateAuthor(Author author)
        {
            try
            {
                var dbAuthor = await GetAutorInfo(author.AuthorId);
                if (dbAuthor == null)
                {
                    return new BasicResponse { Code = 400, Message = $"El autor {author.AuthorId} no existe en la bd" };
                }

                if (!dbAuthor.Name.Equals(author.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (await CheckUniqueEntityText(author.Name))
                    {
                        return new BasicResponse { Code = 400, Message = "Ya existe un autor con el mismo nombre" };
                    }
                }
                

                dbAuthor.Name = author.Name;
                context.Entry(dbAuthor).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = "Update Success",
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.UpdateAuthor");
                return new BasicResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BasicResponse> DeleteAuthor(int AuthorId)
        {
            try
            {

                var dbAuthor = await GetAutorInfo(AuthorId);
                if (dbAuthor == null)
                {
                    return new BasicResponse { Code = 400, Message = $"El autor {AuthorId} no existe en la bd" };
                }

                context.Authors.Remove(dbAuthor);
                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = "Delete Success",
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.DeleteAuthor");
                return new BasicResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        private async Task<Author> GetAutorInfo(int Authorid)
        {
            try
            {
                return await context.Authors.Where(a => a.AuthorId == Authorid).Include(b => b.AuthorBooks).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.GetAutorInfo");
                return null;
            }

        }

        public async Task<bool> CheckUniqueEntityText(string text)
        {
            return await context.Authors.AnyAsync(a => a.Name.ToLower().Equals(text.ToLower()));
        }
    }
}

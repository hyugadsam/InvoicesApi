using DBService.Entities;
using DBService.Interfaces;
using DBService.Utils;
using Dtos.Common;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BasicCreateResponse> Create(Author obj)
        {
            try
            {
                context.Authors.Add(obj);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = obj.AuthorId
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

        public async Task<List<Author>> GetList(PaginationDto pagination)
    {
            try
            {
                return await context.Authors.GetRecordsByPages(pagination).Include(ab => ab.AuthorBooks).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.GetAuthorList");
                return null;
            }
        }

        public async Task<Author> Get(int Id)
        {
            return await GetAutorInfo(Id, true);
        }

        public async Task<BasicResponse> Update(Author obj)
        {
            try
            {
                var dbAuthor = await GetAutorInfo(obj.AuthorId);
                if (dbAuthor == null)
                {
                    return new BasicResponse { Code = 400, Message = $"El autor {obj.AuthorId} no existe en la bd" };
                }

                if (!dbAuthor.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (await CheckUniqueEntityText(obj.Name))
                    {
                        return new BasicResponse { Code = 400, Message = "Ya existe un autor con el mismo nombre" };
                    }
                }
                

                dbAuthor.Name = obj.Name;
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

        public async Task<BasicResponse> Delete(int Id)
        {
            try
            {

                var dbAuthor = await GetAutorInfo(Id);
                if (dbAuthor == null)
                {
                    return new BasicResponse { Code = 400, Message = $"El autor {Id} no existe en la bd" };
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

        public async Task<bool> CheckUniqueEntityText(string text)
        {
            return await context.Authors.AnyAsync(a => a.Name.ToLower().Equals(text.ToLower()));
        }

        private async Task<Author> GetAutorInfo(int Authorid, bool includeChilds = false)
        {
            try
            {
                var query = context.Authors.Where(a => a.AuthorId == Authorid);
                if (includeChilds)
                {
                    query = query.Include(b => b.AuthorBooks).ThenInclude(b => b.Book);
                }
                else
                {
                    query = query.Include(b => b.AuthorBooks);
                }
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.AuthorService.GetAutorInfo");
                return null;
            }

        }

    }
}

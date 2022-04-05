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
    public class ClientService : IClientService
    {
        private readonly DataBaseContext context;
        private readonly ILogger<ClientService> logger;

        public ClientService(DataBaseContext context, ILogger<ClientService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<BasicCreateResponse> Create(Client obj)
        {
            try
            {
                obj.EnrollDate = DateTime.Now;
                context.Clients.Add(obj);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = obj.ClientId
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.ClientService.CreateClient");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<Client>> GetList(PaginationDto pagination)
        {
            try
            {
                var query = context.Clients.GetRecordsByPages(pagination).Include(b => b.ClientBooks);  //Primero el paginado de la entidad principal, luego los include
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.ClientService.GetClientList");
                return null;
            }
        }

        public async Task<Client> Get(int Id)
        {
            return await GetClientInfo(Id, true);
        }

        public async Task<BasicResponse> Update(UpdateClientRequest obj)
        {
            try
            {
                var DbClient = await GetClientInfo(obj.ClientId);
                if (DbClient == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El cliente {obj.ClientId} no existe en la bd"
                    };
                }

                if (!DbClient.Name.Equals(obj.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (await CheckUniqueEntityText(obj.Name))
                    {
                        return new BasicResponse { Code = 400, Message = "Ya existe un cliente con el mismo nombre" };
                    }
                }

                DbClient.Name = obj.Name;
                DbClient.ClientBooks = await context.Books.Where(b => obj.BorrowedBooks != null && obj.BorrowedBooks.Contains(b.BookId))
                    .Select(b => new ClientBook { BookId = b.BookId, ClientId = obj.ClientId }).ToListAsync();
                context.Entry(DbClient).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = "Update Success"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.ClientService.UpdateClient");
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
                var DbClient = await GetClientInfo(Id);
                if (DbClient == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El cliente {Id} no existe en la bd"
                    };
                }

                context.Clients.Remove(DbClient);
                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = "Delete Success"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.ClientService.DeleteClient");
                return new BasicResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        private async Task<Client> GetClientInfo(int Clientid, bool addChilds = false)
        {
            try
            {
                var query = context.Clients.Where(c => c.ClientId == Clientid);
                if (addChilds)
                {
                    query = query.Include(b => b.ClientBooks).ThenInclude(bk => bk.Book);
                }
                else
                {
                    query = query.Include(b => b.ClientBooks);
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.ClientService.GetClientInfo");
                return null;
            }

        }

        public async Task<bool> CheckUniqueEntityText(string text)
        {
            return await context.Clients.AnyAsync(a => a.Name.ToLower().Equals(text.ToLower()));
        }


    }
}

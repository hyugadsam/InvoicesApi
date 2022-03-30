using DBService.Entities;
using DBService.Interfaces;
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

        public async Task<BasicCreateResponse> CreateClient(Client client)
        {
            try
            {
                client.EnrollDate = DateTime.Now;
                context.Clients.Add(client);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = client.ClientId
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

        public async Task<List<Client>> GetClientList()
        {
            try
            {
                return await context.Clients.Include(b => b.BorrowedBooks).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.ClientService.GetClientList");
                return null;
            }
        }

        public async Task<Client> GetClient(int Clientid)
        {
            return await GetClientInfo(Clientid);
        }

        public async Task<BasicResponse> UpdateClient(UpdateClientRequest request)
        {
            try
            {
                var DbClient = await GetClientInfo(request.ClientId);
                if (DbClient == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El cliente {request.ClientId} no existe en la bd"
                    };
                }

                if (!DbClient.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (await CheckUniqueEntityText(request.Name))
                    {
                        return new BasicResponse { Code = 400, Message = "Ya existe un cliente con el mismo nombre" };
                    }
                }

                DbClient.Name = request.Name;
                DbClient.BorrowedBooks = await context.Books.Where(b => request.BorrowedBooks != null ? request.BorrowedBooks.Contains(b.BookId) : false).ToListAsync();
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

        public async Task<BasicResponse> DeleteClient(int Clientid)
        {
            try
            {
                var DbClient = await GetClientInfo(Clientid);
                if (DbClient == null)
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"El cliente {Clientid} no existe en la bd"
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

        private async Task<Client> GetClientInfo(int Clientid)
        {
            try
            {
                return await context.Clients.Where(c => c.ClientId == Clientid).Include(b => b.BorrowedBooks).FirstOrDefaultAsync();
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

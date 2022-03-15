using DBService.Entities;
using Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Dtos.Enums;
using DBService.Interfaces;
using Dtos.Common;
using Dtos.Request;

namespace DBService.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly InvoicesDbContext context;
        private readonly ILogger<InvoicesDbContext> logger;
        private readonly List<int> StatusForBill = new List<int> { 2, 3 }; //Payed, UnBillied
        private readonly List<int> StatusForUnBill = new List<int> { 4 }; //Billed
        private readonly List<int> StatusForPayment = new List<int> { 1 }; //Created


        public TransactionsService(InvoicesDbContext dbContext, ILogger<InvoicesDbContext> logger)
        {
            context = dbContext;
            this.logger = logger;
        }

        public async Task<BasicCreateResponse> CreateTransaction(Transactions transaction)
        {
            try
            {
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();

                return new BasicCreateResponse
                {
                    Code = 200,
                    Message = "Create Success",
                    Id = transaction.TransactionId
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.CreateTransaction");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<Transactions> GetTransaction(long Transactionid)
        {
            try
            {
                return await ExistsTransaction(Transactionid);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.GetTransaction");
                return null;
            }
        }

        public async Task<List<Transactions>> GetTransactionList()
        {
            try
            {
                return await context.Transactions.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.GetTransactionList");
                return null;
            }
        }

        public async Task<BasicResponse> PaymentTransaction(UpdateTransaction transaction)
        {
            try
            {
                var trans = await ExistsTransaction(transaction.TransactionId);
                if (trans == null || !StatusForPayment.Contains(trans.StatusId))
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"Transaction dosent exist or not in valid status for payment ({string.Join(',', GetStatusNames(StatusForPayment))})"
                    };
                }

                var Historic = new TransactionHistoric
                {
                    Comment = transaction.Comment,
                    NewStatusId = transaction.StatusId,
                    OldStatusId = trans.StatusId,
                    TransactionId = transaction.TransactionId,
                };

                trans.StatusId = transaction.StatusId;
                return await UpdateTransaction(trans, Historic, "Payment");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.PaymentTransaction");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BasicResponse> UnBillTransaction(UpdateTransaction transaction)
        {
            try
            {
                var trans = await ExistsTransaction(transaction.TransactionId);
                if (trans == null || !StatusForUnBill.Contains(trans.StatusId))
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"Transaction dosent exist or not in valid status for Un-Bill ({string.Join(',', GetStatusNames(StatusForUnBill))})"
                    };
                }

                var Historic = new TransactionHistoric
                {
                    Comment = transaction.Comment,
                    NewStatusId = transaction.StatusId,
                    OldStatusId = trans.StatusId,
                    TransactionId = transaction.TransactionId,
                };

                trans.StatusId = transaction.StatusId;
                return await UpdateTransaction(trans, Historic, "Un-Bill");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.UnBillTransaction");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BasicResponse> BillTransaction(UpdateTransaction transaction)
        {
            try
            {
                var trans = await ExistsTransaction(transaction.TransactionId);
                if (trans == null || !StatusForBill.Contains(trans.StatusId))
                {
                    return new BasicResponse
                    {
                        Code = 400,
                        Message = $"Transaction dosent exist or not in valid status for Bill ({string.Join(',', GetStatusNames(StatusForBill))})"
                    };
                }

                var Historic = new TransactionHistoric
                {
                    Comment = transaction.Comment,
                    NewStatusId = transaction.StatusId,
                    OldStatusId = trans.StatusId,
                    TransactionId = transaction.TransactionId,
                };

                trans.StatusId = transaction.StatusId;
                return await UpdateTransaction(trans, Historic, "Bill");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.BillTransaction");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<BasicResponse> BillingByRange(DateRangeRequest request)
        {
            try
            {
                //Get the transactions in the range that are availabe for billing
                var transactions = await context.Transactions.Where(t => t.CreatedDate >= request.From
                && t.CreatedDate <= request.To && StatusForBill.Contains(t.StatusId)).ToListAsync();

                if (transactions?.Count == 0)
                {
                    return new BasicResponse
                    {
                        Code=404,
                        Message = "No available transaccions for billing"
                    };
                }

                var statusid = (int)EnumStatus.Billed;
                var NotUpdated = new List<long>();

                foreach (var item in transactions)  //Generate the info and update the status of the each transaction
                {
                    var Historic = new TransactionHistoric
                    {
                        Comment = "Billed by automatic process",
                        NewStatusId = statusid,
                        OldStatusId = item.StatusId,
                        TransactionId = item.TransactionId,
                    };

                    item.StatusId = statusid;

                    var updateResponse = await UpdateTransaction(item, Historic, "Bill");
                    if (updateResponse.Code != 200) //If the transaction wasn't ok, add to the list for review
                    {
                        NotUpdated.Add(item.TransactionId);
                    }
                }

                var Message = $"Operation Completed, transactions billed {transactions.Count - NotUpdated.Count}";
                Message += NotUpdated.Count > 0 ? $"; Transactions not billed: {string.Join(',', NotUpdated)}" : string.Empty;   //if there are pending transaccions, write the list

                return new BasicResponse
                {
                    Code = 200,
                    Message = Message
                };

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.BillingByRange");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }
        
        public async Task<List<TransactionStatus>> GetStatus()
        {
            try
            {
                return await context.TransactionStatus.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.GetStatus");
                return null;
            }
        }

        private async Task<BasicResponse> UpdateTransaction(Transactions transaction, TransactionHistoric Historic, string Operation)
        {
            try
            {
                transaction.LastUpdatedDate = DateTime.Now;
                Historic.Date = transaction.LastUpdatedDate;

                context.Entry(transaction).State = EntityState.Modified;
                context.TransactionHistorics.Add(Historic);

                await context.SaveChangesAsync();

                return new BasicResponse
                {
                    Code = 200,
                    Message = $"{Operation} Success"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in DBService.Services.TransactionsService.UpdateTransaction");
                return new BasicCreateResponse
                {
                    Code = 500,
                    Message = ex.Message
                };
            }
        }

        private List<string> GetStatusNames(List<int> status)
        {
            return Enum.GetValues(typeof(EnumStatus)).Cast<EnumStatus>().Where(v => status.Contains((int)v)).Select(e => e.ToString()).ToList();
        }

        private async Task<Transactions> ExistsTransaction(long TransactionId)
        {
            return await context.Transactions.Include(y => y.Status).Include(x => x.Historic).FirstOrDefaultAsync(r => r.TransactionId == TransactionId);
        }


    }
}

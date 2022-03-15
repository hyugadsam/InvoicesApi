using DBService.Entities;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface ITransactionsService
    {
        Task<BasicCreateResponse> CreateTransaction(Transactions transaction);
        Task<Transactions> GetTransaction(long Transactionid);
        Task<List<Transactions>> GetTransactionList();
        Task<BasicResponse> PaymentTransaction(UpdateTransaction transaction);
        Task<BasicResponse> UnBillTransaction(UpdateTransaction transaction);
        Task<BasicResponse> BillTransaction(UpdateTransaction transaction);
        Task<BasicResponse> BillingByRange(DateRangeRequest request);
        Task<List<TransactionStatus>> GetStatus();
    }
}

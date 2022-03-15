using AutoMapper;
using DBService.Entities;
using DBService.Interfaces;
using DBService.Services;
using Dtos.Common;
using Dtos.Enums;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class AppServiceTransactions
    {
        private readonly IMapper mapper;
        private readonly ITransactionsService service;

        public AppServiceTransactions(InvoicesDbContext dbContext, ILogger<InvoicesDbContext> logger, IMapper mapper)
        {
            service = new TransactionsService(dbContext, logger);
            this.mapper = mapper;
        }

        public async Task<BasicCreateResponse> CreateTransaction(NewTransactionRequest request)
        {
            var transaction = mapper.Map<Transactions>(request);
            return await service.CreateTransaction(transaction);
        }

        public async Task<TransactHistoric> GetTransaction(long Transactionid)
        {
            var result = await service.GetTransaction(Transactionid);
            return mapper.Map<TransactHistoric>(result);
        }

        public async Task<List<Transact>> GetTransactionList()
        {
            var data = await service.GetTransactionList();
            return mapper.Map<List<Transact>>(data);
        }

        public async Task<BasicResponse> PaymentTransaction(UpdateTransactionRequest request)
        {
            var transaction = new UpdateTransaction
            {
                Comment = request.Comment,
                TransactionId = request.TransactionId,
                StatusId = (int) EnumStatus.Payed
            };

            return await service.PaymentTransaction(transaction);
        }

        public async Task<BasicResponse> UnBillTransaction(UpdateTransactionRequest request)
        {
            var transaction = new UpdateTransaction
            {
                Comment = request.Comment,
                TransactionId = request.TransactionId,
                StatusId = (int)EnumStatus.UnBillied
            };

            return await service.UnBillTransaction(transaction);
        }

        public async Task<BasicResponse> BillTransaction(UpdateTransactionRequest request)
        {
            var transaction = new UpdateTransaction
            {
                Comment = request.Comment,
                TransactionId = request.TransactionId,
                StatusId = (int)EnumStatus.Billed
            };

            return await service.BillTransaction(transaction);
        }

        public async Task<BasicResponse> BillingByRange(DateRangeRequest request)
        {
            return await service.BillingByRange(request);
        }

        //Task<List<TransactionStatus>> GetStatus();

    }
}

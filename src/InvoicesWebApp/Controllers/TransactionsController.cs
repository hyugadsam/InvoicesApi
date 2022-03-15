using ApplicationServices.Services;
using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoicesWebApp.Controllers
{
    [Route("api/Transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly AppServiceTransactions service;

        public TransactionsController(AppServiceTransactions appService)
        {
            service = appService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<BasicResponse>> Create(NewTransactionRequest request)
        {
            var response = await service.CreateTransaction(request);
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("Get/{Transactionid}")]
        public async Task<TransactHistoric> GetTransaction(long Transactionid)
        {
            return await service.GetTransaction(Transactionid);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<List<Transact>> GetTransactionList()
        {
            return await service.GetTransactionList();
        }

        [HttpPut]
        [Route("Payment")]
        public async Task<ActionResult<BasicResponse>> PaymentTransaction(UpdateTransactionRequest request)
        {
            var response = await service.PaymentTransaction(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Route("Bill")]
        public async Task<ActionResult<BasicResponse>> BillTransaction(UpdateTransactionRequest request)
        {
            var response = await service.BillTransaction(request);
            return StatusCode(response.Code, response);
        }

        [HttpPut]
        [Route("UnBill")]
        public async Task<ActionResult<BasicResponse>> UnBillTransaction(UpdateTransactionRequest request)
        {
            var response = await service.UnBillTransaction(request);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("BillingByRange")]
        public async Task<ActionResult<BasicResponse>> BillingByRange(DateRangeRequest request)
        {
            var response = await service.BillingByRange(request);
            return StatusCode(response.Code, response);
        }




    }
}

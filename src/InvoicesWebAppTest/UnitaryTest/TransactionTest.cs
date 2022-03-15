using ApplicationServices.Services;
using DBService.Entities;
using Dtos.Enums;
using Dtos.Request;
using InvoicesWebApp.Controllers;
using InvoicesWebAppTest.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesWebAppTest.UnitaryTest
{
    [TestClass]
    public class TransactionTest : BaseTestClass
    {
        [TestMethod]
        public async Task GetTransactionOk()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.GetTransaction(1);

            //Verify
            Assert.AreEqual(1, resp.TransactionId);
            Assert.AreEqual(1, resp.Amount);

        }

        [TestMethod]
        public async Task GetTransactionHistoricOk()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 2,
                Description = "Description",
                LastUpdatedDate = DateTime.Now,
                Historic = new List<TransactionHistoric>() 
                {
                    new TransactionHistoric { Comment = "comment", Date = DateTime.Now, NewStatusId = 2, OldStatusId = 1, TransactionId = 1 }
                }
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.GetTransaction(1);

            //Verify
            Assert.AreEqual(1, resp.TransactionId);
            Assert.AreEqual(1, resp.Amount);
            Assert.AreEqual(1, resp.Historics.Count);

        }

        [TestMethod]
        public async Task GetTransactionFailNotFound()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.GetTransaction(1);

            //Verify
            Assert.AreEqual(null, resp);

        }

        [TestMethod]
        public async Task PayTransactionFailNotCorrectStatus()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 4,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.PaymentTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Payment" });

            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            //Verify
            Assert.AreEqual(400, result.Code);
            Assert.IsTrue(result.Message.Contains(EnumStatus.Created.ToString()));   //The message includes the word Created as part of the error message
        }

        [TestMethod]
        public async Task PayTransactionFailNotExist()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context, logger, mapper));
            var resp = await controller.PaymentTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Payment" });
            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            //Verify
            Assert.AreEqual(400, result.Code);
            Assert.IsTrue(result.Message.Contains(EnumStatus.Created.ToString()));   //The message includes the word Created as part of the error message
        }

        [TestMethod]
        public async Task PayTransactionOk()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.PaymentTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Payment" });

            //Verify
            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);
            Assert.AreEqual(200, result.Code);

            //Another context for the same db
            var context3 = CreateDbContext(DbName);
            var trans = await context3.Transactions.Include(x => x.Historic).FirstAsync();

            Assert.IsNotNull(trans);
            Assert.AreEqual((int)EnumStatus.Payed, trans.StatusId);
            Assert.IsTrue(trans.Historic.First().OldStatusId == (int)EnumStatus.Created && trans.Historic.First().NewStatusId == (int)EnumStatus.Payed);

        }

        [TestMethod]
        public async Task BillTransactionOk()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();


            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.PaymentTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Payment" });

            //Verify
            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);
            Assert.AreEqual(200, result.Code);

            //Another context for the same db
            var context3 = CreateDbContext(DbName);

            //Prueba
            var controller2 = new TransactionsController(new AppServiceTransactions(context3, logger, mapper));
            var resp2 = await controller.BillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Bill" });

            var result2 = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);
            Assert.AreEqual(200, result2.Code);

            //Another context for the same db
            var context4 = CreateDbContext(DbName);
            var trans = await context3.Transactions.Include(x => x.Historic).FirstAsync();
            Assert.IsNotNull(trans);
            Assert.AreEqual((int)EnumStatus.Billed, trans.StatusId);
            Assert.IsTrue(trans.Historic.Last().OldStatusId == (int)EnumStatus.Payed && trans.Historic.Last().NewStatusId == (int)EnumStatus.Billed);


        }

        [TestMethod]
        public async Task BillTransactionFailNotCorrectStatus()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.BillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Bill" });

            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            //Verify
            Assert.AreEqual(400, result.Code);
            Assert.IsTrue(result.Message.Contains(EnumStatus.Payed.ToString()));  //The message includes the word Payed as part of the error message
            

        }

        [TestMethod]
        public async Task BillTransactionFailNotExist()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context, logger, mapper));
            var resp = await controller.BillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Bill" });

            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            //Verify
            Assert.AreEqual(400, result.Code);
            Assert.IsTrue(result.Message.Contains(EnumStatus.Payed.ToString()));   //The message includes the word Created as part of the error message
        }

        [TestMethod]
        public async Task UnBillTransactionOk()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.PaymentTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Payment" });

            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);


            //Verify
            Assert.AreEqual(200, result.Code);

            //Another context for the same db
            var context3 = CreateDbContext(DbName);

            //Prueba
            var controller2 = new TransactionsController(new AppServiceTransactions(context3, logger, mapper));
            var resp2 = await controller.BillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Bill" });

            var result2 = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            Assert.AreEqual(200, result2.Code);

            //Another context for the same db
            var context4 = CreateDbContext(DbName);

            //Prueba
            var controller3 = new TransactionsController(new AppServiceTransactions(context4, logger, mapper));
            var resp3 = await controller.UnBillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Un-Bill" });

            var result3 = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);
            
            Assert.AreEqual(200, result3.Code);

            //Another context for the same db
            var context5 = CreateDbContext(DbName);

            var trans = await context5.Transactions.Include(x => x.Historic).FirstAsync();
            Assert.IsNotNull(trans);
            Assert.AreEqual((int)EnumStatus.UnBillied, trans.StatusId);
            Assert.IsTrue(trans.Historic.Last().OldStatusId == (int)EnumStatus.Billed && trans.Historic.Last().NewStatusId == (int)EnumStatus.UnBillied);


        }

        [TestMethod]
        public async Task UnBillTransactionFailNotCorrectStatus()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                StatusId = 1,
                Description = "Description",
                LastUpdatedDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            //Another context for the same db
            var context2 = CreateDbContext(DbName);

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context2, logger, mapper));
            var resp = await controller.UnBillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Un-Bill" });

            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            //Verify
            Assert.AreEqual(400, result.Code);
            Assert.IsTrue(result.Message.Contains(EnumStatus.Billed.ToString()));  //The message includes the word Billed as part of the error message

        }

        [TestMethod]
        public async Task UnBillTransactionFailNotExist()
        {
            #region Prepare

            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);
            var mapper = ConfigureMapper();
            var logger = LoggerMok<InvoicesDbContext>.ConfigureLogger();

            #endregion

            //Prueba
            var controller = new TransactionsController(new AppServiceTransactions(context, logger, mapper));
            var resp = await controller.UnBillTransaction(new UpdateTransactionRequest { TransactionId = 1, Comment = "Test Un-Bill" });

            var result = Converter.GetObjectResultContent<Dtos.Responses.BasicResponse>(resp.Result);

            //Verify
            Assert.AreEqual(400, result.Code);
            Assert.IsTrue(result.Message.Contains(EnumStatus.Billed.ToString()));   //The message includes the word Created as part of the error message
        }


    }
}

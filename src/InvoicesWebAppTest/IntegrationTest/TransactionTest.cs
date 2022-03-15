using DBService.Entities;
using Dtos.Common;
using Dtos.Enums;
using Dtos.Request;
using Dtos.Responses;
using InvoicesWebAppTest.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesWebAppTest.IntegrationTest
{
    [TestClass]
    public class TransactionTest: BaseTestClass
    {
        private readonly string url = "api/Transactions";

        [TestMethod]
        public async Task GetTransactionOk()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                Description = "First",
                LastUpdatedDate = DateTime.Now,
                StatusId = 1
            });

            await context.SaveChangesAsync();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();
            
            var respuest = await client.GetAsync($"{url}/Get/{1}");

            ////Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<TransactHistoric>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(1, response.TransactionId);
            Assert.IsFalse(string.IsNullOrEmpty(response.Description));
        }

        [TestMethod]
        public async Task GetTransactionNotExists()
        {
            var DbName = Guid.NewGuid().ToString();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var respuest = await client.GetAsync($"{url}/Get/{1}");

            ////Prueba
            Assert.AreEqual(HttpStatusCode.NoContent, respuest.StatusCode);
        }


        [TestMethod]
        public async Task GetTransactionListOK()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            context.Transactions.AddRange(new List<Transactions>()
            {
                new Transactions
                {
                    Amount = 1,
                    CreatedDate = DateTime.Now,
                    Description = "First",
                    LastUpdatedDate = DateTime.Now,
                    StatusId = 1
                },
                new Transactions
                {
                    Amount = 2,
                    CreatedDate = DateTime.Now,
                    Description = "Second",
                    LastUpdatedDate = DateTime.Now,
                    StatusId = 1
                }
            });

            await context.SaveChangesAsync();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var respuest = await client.GetAsync($"{url}/Get");

            ////Prueba
            var response = JsonConvert.DeserializeObject<List<Transact>>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(2, response.Count);
        }

        [TestMethod]
        public async Task GetTransactionListNoContent()
        {
            var DbName = Guid.NewGuid().ToString();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var respuest = await client.GetAsync($"{url}/Get");

            //Prueba
            var response = JsonConvert.DeserializeObject<List<Transact>>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public async Task CreateTransactionOk()
        {
            var nombre = Guid.NewGuid().ToString();
            var context = CreateDbContext(nombre);
            
            var factory = GetWebAppFactory(nombre);
            var client = factory.CreateClient();

            var request = new NewTransactionRequest
            {
                Amount = float.MaxValue,
                Description = "Description"
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync($"{url}/Create", httpContent);

            ////Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicCreateResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);
            Assert.AreEqual(1, response.Id);

            var trans = await context.Transactions.FirstAsync();
            Assert.AreEqual(trans.Amount, float.MaxValue);
        }

        [TestMethod]
        public async Task CreateTransactionFailAmount()
        {
            var nombre = Guid.NewGuid().ToString();
            var factory = GetWebAppFactory(nombre);
            var client = factory.CreateClient();

            var request = new NewTransactionRequest
            {
                Amount = float.MinValue,
                Description = "Description"
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PostAsync($"{url}/Create", httpContent);
            var message = await respuest.Content.ReadAsStringAsync();
            //Prueba

            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);
            Assert.IsTrue(message.Contains("Amount"));
        }


        [TestMethod]
        public async Task PayTransactionOk()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                Description = "First",
                LastUpdatedDate = DateTime.Now,
                StatusId = 1
            });

            await context.SaveChangesAsync();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var request = new UpdateTransactionRequest
            {
                Comment = "Payment Test",
                TransactionId = 1
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Payment", httpContent);

            //Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);

            //Prueba
            var context2 = CreateDbContext(DbName);
            var trans = await context2.Transactions.Include(x => x.Historic).FirstAsync();
            Assert.AreEqual(1, trans.Historic.Count);
            Assert.AreEqual(trans.StatusId, (int)EnumStatus.Payed);
        }

        [TestMethod]
        public async Task PayTransactionFail()
        {
            var DbName = Guid.NewGuid().ToString();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var request = new UpdateTransactionRequest
            {
                Comment = "Payment Test",
                TransactionId = 1
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Payment", httpContent);
            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);


            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains(EnumStatus.Created.ToString()));
        }

        [TestMethod]
        public async Task BillTransactionOk()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                Description = "First",
                LastUpdatedDate = DateTime.Now,
                StatusId = 2
            });

            await context.SaveChangesAsync();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var request = new UpdateTransactionRequest
            {
                Comment = "Bill Test",
                TransactionId = 1
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Bill", httpContent);

            //Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);

            //Prueba
            var context2 = CreateDbContext(DbName);
            var trans = await context2.Transactions.Include(x => x.Historic).FirstAsync();
            Assert.AreEqual(1, trans.Historic.Count);
            Assert.AreEqual(trans.StatusId, (int)EnumStatus.Billed);
        }

        [TestMethod]
        public async Task BillTransactionFail()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var request = new UpdateTransactionRequest
            {
                Comment = "Bill Test",
                TransactionId = 1
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/Bill", httpContent);
            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);


            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains(EnumStatus.Payed.ToString()));
        }

        [TestMethod]
        public async Task UnBillTransactionOk()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            context.Transactions.Add(new Transactions
            {
                Amount = 1,
                CreatedDate = DateTime.Now,
                Description = "First",
                LastUpdatedDate = DateTime.Now,
                StatusId = 4
            });

            await context.SaveChangesAsync();

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var request = new UpdateTransactionRequest
            {
                Comment = "UnBill Test",
                TransactionId = 1
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/UnBill", httpContent);

            //Prueba
            respuest.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(200, response.Code);

            //Prueba
            var context2 = CreateDbContext(DbName);
            var trans = await context2.Transactions.Include(x => x.Historic).FirstAsync();
            Assert.AreEqual(1, trans.Historic.Count);
            Assert.AreEqual(trans.StatusId, (int)EnumStatus.UnBillied);
        }

        [TestMethod]
        public async Task UnBillTransactionFail()
        {
            var DbName = Guid.NewGuid().ToString();
            var context = CreateDbContext(DbName);

            //Creates the webapp instance an the client
            var factory = GetWebAppFactory(DbName);
            var client = factory.CreateClient();

            var request = new UpdateTransactionRequest
            {
                Comment = "UnBill Test",
                TransactionId = 1
            };
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var respuest = await client.PutAsync($"{url}/UnBill", httpContent);
            Assert.AreEqual("Bad Request", respuest.ReasonPhrase);

            var response = JsonConvert.DeserializeObject<BasicResponse>(await respuest.Content.ReadAsStringAsync());
            Assert.AreEqual(400, response.Code);
            Assert.IsTrue(response.Message.Contains(EnumStatus.Billed.ToString()));
        }


    }
}

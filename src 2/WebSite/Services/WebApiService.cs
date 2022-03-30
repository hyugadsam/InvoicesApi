using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebSite.Models.Enums;

namespace WebSite.Services
{
    public class WebApiService : IWebApiService
    {
        private readonly HttpClient client;

        public WebApiService(HttpClient client)
        {
            this.client = client;
        }

        #region Authors

        public async Task<List<AuthorDto>> GetAuthorList()
        {
            
            var resp = await client.GetAsync(AuthorsRoutes.GetAuthorList);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new List<AuthorDto>();


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AuthorDto>>(response);

        }

        public async Task<BasicCreateResponse> CreateAuthor(string Name)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(Name), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(AuthorsRoutes.CreateAuthor, content);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicCreateResponse() { Code = 500, Message = "Error de comunicacion con la api" };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicCreateResponse>(response);

        }

        public async Task<FullAuthorDto> GetAuthor(int Authorid)
        {
            var resp = await client.GetAsync(string.Format(AuthorsRoutes.GetAuthor, Authorid));
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return new FullAuthorDto();
            }

            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FullAuthorDto>(response);

        }


        public async Task<BasicResponse> UpdateAuthor(AuthorDto author)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(author), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync(AuthorsRoutes.UpdateAuthor, content);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicResponse() { Code = 500, Message = "Error de comunicacion con la api" };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicResponse>(response);

        }

        public async Task<BasicResponse> Delete(int Authorid)
        {
            var resp = await client.DeleteAsync(string.Format(AuthorsRoutes.Delete, Authorid));
            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicResponse() { Code = 500, Message = "Error de comunicacion con la api al tratar de eliminar" };

            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicResponse>(response);
        }

        #endregion

        #region Books

        public async Task<List<BookDto>> GetBookList()
        {
            var resp = await client.GetAsync(BooksRoutes.GetBookList);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new List<BookDto>();


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<BookDto>>(response);

        }

        public async Task<BasicCreateResponse> CreateBook(string Title)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(Title), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(BooksRoutes.CreateBook, content);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicCreateResponse() { Code = 500, Message = "Error de comunicacion con la api al crear el libro" };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicCreateResponse>(response);
        }

        public async Task<FullBookDto> GetBook(int BookId)
        {
            var resp = await client.GetAsync(string.Format(BooksRoutes.GetBook, BookId));
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return new FullBookDto();
            }

            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FullBookDto>(response);
        }

        public async Task<BasicResponse> UpdateBook(UpdateBookRequest request)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync(BooksRoutes.UpdateBook, content);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicResponse() { Code = 500, Message = "Error de comunicacion con la api al actualizar el libro" };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicResponse>(response);
        }

        public async Task<BasicResponse> DeleteBook(int BookId)
        {
            var resp = await client.DeleteAsync(string.Format(BooksRoutes.Delete, BookId));
            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicResponse() { Code = 500, Message = "Error de comunicacion con la api al tratar de eliminar" };

            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicResponse>(response);
        }

        #endregion

        #region Clients

        public async Task<List<ClientDto>> GetClientList()
        {
            var resp = await client.GetAsync(ClientsRoutes.GetClientList);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new List<ClientDto>();


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ClientDto>>(response);

        }

        public async Task<BasicCreateResponse> CreateClient(string Name)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(Name), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(ClientsRoutes.CreateClient, content);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicCreateResponse() { Code = 500, Message = "Error de comunicacion con la api al crear el libro" };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicCreateResponse>(response);
        }

        public async Task<FullClientDto> GetClient(int Clientid)
        {
            var resp = await client.GetAsync(string.Format(ClientsRoutes.GetClient, Clientid));
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return new FullClientDto();
            }

            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FullClientDto>(response);
        }

        public async Task<BasicResponse> UpdateClient(UpdateClientRequest request)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var resp = await client.PutAsync(ClientsRoutes.UpdateClient, content);

            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicResponse() { Code = 500, Message = "Error de comunicacion con la api al actualizar el libro" };


            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicResponse>(response);
        }

        public async Task<BasicResponse> DeleteClient(int Clientid)
        {
            var resp = await client.DeleteAsync(string.Format(ClientsRoutes.Delete, Clientid));
            if (resp.StatusCode != HttpStatusCode.OK)
                return new BasicResponse() { Code = 500, Message = "Error de comunicacion con la api al tratar de eliminar" };

            var response = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasicResponse>(response);
        }









        #endregion

    }
}

using Dtos.Common;
using Dtos.Request;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Services
{
    public interface IWebApiService
    {
        #region Authors

        Task<List<AuthorDto>> GetAuthorList();
        Task<BasicCreateResponse> CreateAuthor(string Name);
        Task<FullAuthorDto> GetAuthor(int Authorid);
        Task<BasicResponse> UpdateAuthor(AuthorDto author);
        Task<BasicResponse> Delete(int Authorid);

        #endregion

        #region Books

        Task<List<BookDto>> GetBookList();
        Task<BasicCreateResponse> CreateBook(string Title);
        Task<FullBookDto> GetBook(int BookId);
        Task<BasicResponse> UpdateBook(UpdateBookRequest request);
        Task<BasicResponse> DeleteBook(int BookId);


        #endregion

        #region Clients

        Task<List<ClientDto>> GetClientList();
        Task<BasicCreateResponse> CreateClient(string Name);
        Task<FullClientDto> GetClient(int Clientid);
        Task<BasicResponse> UpdateClient(UpdateClientRequest request);
        Task<BasicResponse> DeleteClient(int Clientid);

        #endregion

    }
}

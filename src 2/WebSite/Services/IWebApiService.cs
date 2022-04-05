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

        Task<List<AuthorDto>> GetAuthorList(Models.Common.PaginationDto pagination);
        Task<BasicCreateResponse> CreateAuthor(string Name);
        Task<FullAuthorDto> GetAuthor(int Authorid);
        Task<BasicResponse> UpdateAuthor(AuthorDto author);
        Task<BasicResponse> DeleteAuthor(int Authorid);

        #endregion

        #region Books

        Task<List<BookDto>> GetBookList(Models.Common.PaginationDto pagination);
        Task<BasicCreateResponse> CreateBook(string Title);
        Task<FullBookDto> GetBook(int BookId);
        Task<BasicResponse> UpdateBook(UpdateBookRequest request);
        Task<BasicResponse> DeleteBook(int BookId);


        #endregion

        #region Clients

        Task<List<ClientDto>> GetClientList(Models.Common.PaginationDto pagination);
        Task<BasicCreateResponse> CreateClient(string Name);
        Task<FullClientDto> GetClient(int Clientid);
        Task<BasicResponse> UpdateClient(UpdateClientRequest request);
        Task<BasicResponse> DeleteClient(int Clientid);

        #endregion

    }
}

using DBService.Entities;
using Dtos.Request;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IBookService: IValidateUniqueText
    {
        Task<BasicCreateResponse> CreateBook(Book book);
        Task<List<Book>> GetBookList();
        Task<Book> GetBook(int BookId);
        Task<BasicResponse> UpdateBook(UpdateBookRequest request);
        Task<BasicResponse> DeleteBook(int BookId);

    }

}

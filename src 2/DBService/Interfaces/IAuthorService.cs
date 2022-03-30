using DBService.Entities;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces
{
    public interface IAuthorService: IValidateUniqueText
    {
        Task<BasicCreateResponse> CreateAuthor(Author author);
        Task<List<Author>> GetAuthorList();
        Task<Author> GetAuthor(int Authorid);
        Task<BasicResponse> UpdateAuthor(Author author);
        Task<BasicResponse> DeleteAuthor(int AuthorId);

    }

}

using Dtos.Common;
using Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBService.Interfaces.CommonInterfaces
{
    public interface IBasicOperations<T> where T : class
    {
        Task<BasicCreateResponse> Create(T obj);
        Task<List<T>> GetList(PaginationDto pagination);
        Task<T> Get(int Id);

    }
}

using Dtos.Responses;
using System.Threading.Tasks;

namespace WebSite.Interfaces.Common
{
    public interface IModifyOperations<T> where T : class
    {
        public Task<BasicResponse> Update(T obj);
        public Task<BasicResponse> Delete(int Id);

    }
}

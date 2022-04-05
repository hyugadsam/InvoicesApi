using Dtos.Responses;
using System.Threading.Tasks;

namespace WebSite.Interfaces.Common
{
    public interface ICreate<T> where T : class
    {
        public Task<BasicCreateResponse> Create(T obj);
    }

}

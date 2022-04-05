using System.Threading.Tasks;

namespace WebSite.Interfaces.Common
{
    public interface IGetItem<T> where T : class
    {
        public Task<T> Get(int Id);
    }
}

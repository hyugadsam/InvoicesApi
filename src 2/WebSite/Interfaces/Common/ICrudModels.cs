using System.Threading.Tasks;

namespace WebSite.Interfaces.Common
{
    public interface ICrudModels<T, U> where T : class
    {
        Task<T> GetUpdateModel(int Id);
    }
}

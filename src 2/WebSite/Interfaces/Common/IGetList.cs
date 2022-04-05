using WebSite.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Interfaces.Common
{
    public interface IGetList<T> where T : class
    {
        public Task<List<T>> GetList(PaginationDto pagination);

    }
}

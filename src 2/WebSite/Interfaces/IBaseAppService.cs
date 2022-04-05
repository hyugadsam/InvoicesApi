using WebSite.Models.Common;
using System.Collections.Generic;

namespace WebSite.Interfaces
{
    public interface IBaseAppService
    {
        PaginationDto GetDefaultPagination();
        PaginationDto GetFullList();
        int GetRecordsPerPage();
        List<int> GetRecordsList();
    }
}

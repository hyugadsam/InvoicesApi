using Dtos.Common;
using WebSite.Interfaces.Common;
using WebSite.Models.Authors;

namespace WebSite.Interfaces
{
    public interface IAuthorsAppService : IGetList<AuthorDto>, IGetItem<FullAuthorDto>, IModifyOperations<AuthorDto>, ICreate<NewAuthorModel>
    {
    }

}

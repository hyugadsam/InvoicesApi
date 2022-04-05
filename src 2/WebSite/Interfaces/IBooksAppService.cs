using Dtos.Common;
using WebSite.Interfaces.Common;
using WebSite.Models.Books;

namespace WebSite.Interfaces
{
    public interface IBooksAppService : IGetList<BookDto>, IGetItem<FullBookDto>, IModifyOperations<UpdateBookModel>,
                                        ICreate<NewBookModel>, ICrudModels<UpdateBookModel, string>
    {
    }

}

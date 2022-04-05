using Dtos.Common;

namespace WebSite.Models.Authors
{
    /// <summary>
    /// Clas for select a list of authors
    /// </summary>
    public class SelectAuthorModel
    {
        public bool Check { get; set; }
        public AuthorDto Author { get; set; }
    }
}

using Dtos.Common;

namespace WebSite.Models.Books
{
    public class SelectBookModel
    {
        public bool Checked { get; set; }
        public BookDto Book { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DBService.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
    }
}

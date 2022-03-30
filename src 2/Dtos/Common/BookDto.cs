using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class BookDto
    {
        public int BookId { get; set; }

        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

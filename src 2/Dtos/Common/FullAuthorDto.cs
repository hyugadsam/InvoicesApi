using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class FullAuthorDto : AuthorDto
    {
        public List<BookDto> BooksList { get; set; }

    }
}

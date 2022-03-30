using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class FullClientDto : ClientDto
    {
        public List<BookDto> BorrowedBooks { get; set; }
    }
}

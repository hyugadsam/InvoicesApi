using Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Request
{
    public class UpdateBookRequest: BookDto
    {
        public List<int> AuthorsList { get; set; }
    }
}

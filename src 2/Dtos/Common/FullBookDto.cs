using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class FullBookDto : BookDto
    {
        public List<AuthorDto> AuthorsList { get; set; }
    }
}

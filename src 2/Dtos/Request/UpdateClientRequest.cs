using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Request
{
    public class UpdateClientRequest
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public List<int> BorrowedBooks { get; set;}
    }
}

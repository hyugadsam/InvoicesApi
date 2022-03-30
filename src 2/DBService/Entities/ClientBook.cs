using System;
using System.Collections.Generic;
using System.Text;

namespace DBService.Entities
{
    public class ClientBook
    {
        public int ClientId { get; set; }
        public int BookId { get; set; }
        public Client Client { get; set; }
        public Book Book { get; set; }
    }
}

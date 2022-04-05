using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBService.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        public DateTime CreateDate { get; set; }

        public List<AuthorBook> AuthorBooks { get; set; }

        //public Client Client { get; set; }

        public int Quantity { get; set; }

        public List<ClientBook> ClientBooks { get; set; }

    }
}

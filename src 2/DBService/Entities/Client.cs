using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DBService.Entities
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        [Required, MaxLength(250)]
        public string Name { get; set; }
        [Required]
        public DateTime EnrollDate { get; set; }

        //public List<Book> BorrowedBooks { get; set; }

        public List<ClientBook> ClientBooks { get; set; }
    }
}

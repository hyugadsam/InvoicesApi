using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebSite.Models.Books;

namespace WebSite.Models.Clients
{
    public class UpdateClientModel
    {
        public int ClientId { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [MinLength(5, ErrorMessage = "Longitud minima de 5")]
        public string Name { get; set; }
        public List<SelectBookModel> BookList { get; set; }

        [BindProperty]
        public List<int> BooksId { get; set; }
    }
}

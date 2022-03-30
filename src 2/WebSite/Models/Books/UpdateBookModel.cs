using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebSite.Models.Authors;

namespace WebSite.Models.Books
{
    public class UpdateBookModel
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "El campo titulo es requerido")]
        [MinLength(5, ErrorMessage = "Longitud minima de 5")]
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }

        public List<SelectAuthorModel> AuthorList { get; set; }

        [BindProperty]
        public List<int> AuthorsId { get; set; }
    }
}

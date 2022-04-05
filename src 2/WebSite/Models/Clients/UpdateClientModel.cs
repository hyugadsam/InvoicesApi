using Dtos.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebSite.Models.Books;

namespace WebSite.Models.Clients
{
    public class UpdateClientModel
    {
        public UpdateClientModel()
        {
            BookList = new List<SelectBookModel>();
        }

        public int ClientId { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [MinLength(5, ErrorMessage = "Longitud minima de 5")]
        public string Name { get; set; }

        public List<SelectBookModel> BookList { get; set; }

        [BindProperty]
        public List<int> BooksId { get; set; }


        public void MarkSelectedBooks(List<BookDto> FullList, List<BookDto> AssingedList)
        {
            var list = new List<SelectBookModel>();
            if (FullList == null || FullList?.Count() == 0)
                return;

            list = FullList.Select(b => new SelectBookModel
            {
                Book = b,
                Checked = AssingedList != null && AssingedList.Any(bk => bk.BookId == b.BookId) //Marcar los asignados
            }).ToList();

            this.BookList = list;
        }


    }
}

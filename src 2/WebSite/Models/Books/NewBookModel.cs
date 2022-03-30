using System.ComponentModel.DataAnnotations;

namespace WebSite.Models.Books
{
    public class NewBookModel
    {
        [Required(ErrorMessage = "El titulo es requerido")]
        [MinLength(5, ErrorMessage = "La longitud minima es de 5")]
        public string Title { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebSite.Models.Authors
{
    public class NewAuthorModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MinLength(5, ErrorMessage = "La longitud minima es de 5")]
        public string Name { get; set; }
    }
}

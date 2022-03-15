using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dtos.Request
{
    public class NewTransactionRequest
    {
        [Required]
        [Range(0.1, float.MaxValue)]
        public float Amount { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }
    }
}

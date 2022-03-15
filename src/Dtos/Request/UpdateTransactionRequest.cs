using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dtos.Request
{
    public class UpdateTransactionRequest
    {
        [Required]
        [MaxLength(250)]
        public string Comment { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public long TransactionId { get; set; }
    }
}

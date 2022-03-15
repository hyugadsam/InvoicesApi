using System;
using System.Collections.Generic;
using System.Text;

namespace DBService.Entities
{
    public class Transactions
    {
        public long TransactionId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public int StatusId { get; set; }

        public float Amount { get; set; }

        public string Description { get; set; }

        public virtual List<TransactionHistoric> Historic { get; set; }

        public virtual TransactionStatus Status { get; set; }

    }
}

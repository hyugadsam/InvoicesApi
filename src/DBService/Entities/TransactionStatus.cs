using System;
using System.Collections.Generic;
using System.Text;

namespace DBService.Entities
{
    public class TransactionStatus
    {
        public int StatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Transactions> Transaction { get; set; }
        public virtual ICollection<TransactionHistoric> TransactionHistorics { get; set; }

    }
}

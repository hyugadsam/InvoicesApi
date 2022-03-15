using System;
using System.Collections.Generic;
using System.Text;

namespace DBService.Entities
{
    public class TransactionHistoric
    {
        public long Id { get; set; }

        public long TransactionId { get; set; }

        public DateTime Date { get; set; }

        public int OldStatusId { get; set; }

        public int NewStatusId { get; set; }

        public string Comment { get; set; }

        public virtual Transactions Transaction { get; set; }

        public virtual TransactionStatus Status { get; set; }
    }
}

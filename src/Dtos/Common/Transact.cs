using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class Transact
    {
        public long TransactionId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string Status { get; set; }

        public float Amount { get; set; }

        public string Description { get; set; }


        
    }
}

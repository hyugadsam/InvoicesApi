using Dtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class UpdateTransaction: UpdateTransactionRequest
    {
        public int StatusId { get; set; }
    }
}

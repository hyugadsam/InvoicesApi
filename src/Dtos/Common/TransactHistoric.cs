using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class TransactHistoric : Transact
    {
        public List<Historic> Historics { get; set; }
    }
}

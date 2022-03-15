using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class Historic
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string OldStatus { get; set; }

        public string NewStatus { get; set; }

        public string Comment { get; set; }
    }
}

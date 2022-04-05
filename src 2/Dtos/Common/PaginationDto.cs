using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Common
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        private int _recordsPerPage = 10;
        private readonly int MaxRecordsPerPage = 50;

        public int RecordsPerPage
        {
            get { return _recordsPerPage; }
            set { _recordsPerPage = (value > MaxRecordsPerPage || value <= 0) ? MaxRecordsPerPage : value; }
        }

    }
}

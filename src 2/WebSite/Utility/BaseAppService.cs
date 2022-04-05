using WebSite.Models.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using WebSite.Interfaces;

namespace WebSite.Utility
{
    public class BaseAppService: IBaseAppService
    {
        private readonly IConfiguration configuration;

        public BaseAppService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public PaginationDto GetDefaultPagination()
        {
            return new PaginationDto { Page = 1, RecordsPerPage = GetRecordsPerPage() };
        }

        public PaginationDto GetFullList()
        {
            return new PaginationDto { Page = 1, RecordsPerPage = GetMaxRecordsPerPage() };
        }

        public List<int> GetRecordsList()
        {
            string data = configuration["ListRecordsPerPager"];
            return data.Split(',').Select(a => Convert.ToInt32(a) ).ToList();
        }

        public int GetRecordsPerPage()
        {
            int.TryParse(configuration["DefaultRecordsPerPage"], out int RecordsPerPage);

            return RecordsPerPage;
        }

        public int GetMaxRecordsPerPage()
        {
            int.TryParse(configuration["MaxRecordsPerPage"], out int RecordsPerPage);

            return RecordsPerPage;
        }

    }
}

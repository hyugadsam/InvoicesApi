using ApplicationServices.Utilities;
using AutoMapper;
using DBService.Entities;
using Dtos.Enums;
using InvoicesWebApp;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesWebAppTest.Utilities
{
    public class BaseTestClass
    {
        protected InvoicesDbContext CreateDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<InvoicesDbContext>().UseInMemoryDatabase(name).Options;
            var dbContext = new InvoicesDbContext(options);
            return dbContext;

        }

        protected IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });
            return config.CreateMapper();
        }

        protected IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).Build();
        }

        protected WebApplicationFactory<Startup> GetWebAppFactory(string DBName)
        {
            var factory = new WebApplicationFactory<Startup>();

            factory = factory.WithWebHostBuilder(webHostBuilder =>
            {
                webHostBuilder.ConfigureTestServices(services =>
                {
                    var descriptorDBContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<InvoicesDbContext>));
                    if (descriptorDBContext != null)
                    {
                        services.Remove(descriptorDBContext);
                    }

                    services.AddDbContext<InvoicesDbContext>(options => options.UseInMemoryDatabase(DBName));
                });
            });

            return factory;
        }

    }
}

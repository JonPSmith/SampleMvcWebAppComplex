using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataLayer.GeneratedEf;
using DelegateDecompiler;
using NUnit.Framework;
using Tests.DtoClasses;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test99ArticleItems
    {

        //[Test]
        //public void Test01ListCustomersViaLinqPartsOk()
        //{
        //    using (var db = new AdventureWorksLt2012())
        //    {
        //        //SETUP
        //        var log = new List<string>();
        //        var pageSize = 5;
        //        var pageNumber = 0;
        //        var realCustomers = db.Customers.Where(c => c.SalesOrderHeaders.Any());
        //        var details = realCustomers.Select(x => new
        //        {
        //            x.CompanyName,
        //            ContactName = (x.Title ?? "") + " " + x.FirstName + " " + (x.MiddleName ?? "") + " " + x.LastName + " " + (x.Suffix ?? "")
        //        });
        //        var filteredQuery = details.Where(x => x.CompanyName.Contains("Bike"));
        //        var pagedQuery = filteredQuery.OrderBy(x => x.CompanyName).Skip(pageSize * pageNumber).Take(pageSize);

        //        db.Database.Log = log.Add;

        //        //ATTEMPT
        //        var customers = pagedQuery.ToList();

        //        //VERIFY
        //        customers.Count.ShouldBeGreaterThan(0);
        //        foreach (var line in log)
        //        {
        //            Console.WriteLine(line);
        //        }
        //    }
        //}

        [Test]
        public void Test10ListCustomersViaLinqPartsNoNullOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var log = new List<string>();
                var pageSize = 5;
                var pageNumber = 0;
                var realCustomers = db.Customers.Where(c => c.SalesOrderHeaders.Any());
                var details = realCustomers.Select(x => new
                {
                    x.CompanyName,
                    ContactName = x.Title + " " + x.FirstName + " " + x.MiddleName + " " + x.LastName + " " + x.Suffix
                });
                var filteredQuery = details.Where(x => x.CompanyName.Contains("Bike"));
                var pagedQuery = filteredQuery.OrderBy(x => x.CompanyName).Skip(pageSize * pageNumber).Take(pageSize);

                db.Database.Log = log.Add;

                //ATTEMPT
                var customers = pagedQuery.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
                foreach (var line in log)
                {
                    Console.WriteLine(line);
                }
            }
        }


        [Test]
        public void Test20ListCustomersViaAutoMapperOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var log = new List<string>();
                var pageSize = 5;
                var pageNumber = 0;
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, TestListCustomerDto>()
                    .ForMember(d => d.TotalAllOrders,
                    opt => opt.MapFrom(c => c.SalesOrderHeaders.Sum(x => (decimal?) x.TotalDue) ?? 0)));

                var custs = db.Customers.ProjectTo<TestListCustomerDto>(config).Decompile();
                var filteredQuery = custs.Where(x => x.CompanyName.Contains("Bike") && x.TotalAllOrders > 0);
                var pagedQuery = filteredQuery.OrderBy(x => x.CompanyName).Skip(pageSize * pageNumber).Take(pageSize);

                db.Database.Log = log.Add;

                //ATTEMPT
                var customers = pagedQuery.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
                foreach (var line in log)
                {
                    Console.WriteLine(line);
                }
            }
        }

        [Test]
        public void Test99ArticleOk()
        {


            var query = new[] { "Fred", "Bert", "Joe" }.Where(s => s.StartsWith("B"));
            var str = query.ToString();
            var ans = query.ToList();

        }


    }
}

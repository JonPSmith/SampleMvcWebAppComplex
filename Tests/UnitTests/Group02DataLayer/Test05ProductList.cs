using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.GeneratedEf;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.UnitTests.Group02DataLayer
{
    class Test05ProductList
    {



        [Test]
        public void Test01ListCustomerOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var now = DateTime.Now;

                //ATTEMPT
                var list = db.Products.Where(x => x.SellStartDate < now && (now <= (x.SellEndDate ?? now)) ).ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
            }
        }

    }
}

#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test01ModifiedDate.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.UnitTests.Group02DataLayer
{
    class Test01ModifiedDate
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            //we do this to force EF setup otherwise the timing below fails
            using (var db = new AdventureWorksLt2012())
            {
                db.ProductDescriptions.ToList();
            }
        }

        private const int HalfSecondInTicks = 10000000/2;

        [Test]
        public void Test01TrackedEntitiesAddOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var description = new ProductDescription
                {
                    Description = Guid.NewGuid().ToString()
                };

                //ATTEMPT
                db.ProductDescriptions.Add(description);
                var status = db.SaveChangesWithChecking();

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                description.ModifiedDate.Ticks.ShouldEqualWithTolerance(DateTime.UtcNow.Ticks, HalfSecondInTicks);
                description.rowguid.ShouldNotEqual( new Guid());
            }
        }

        [Test]
        public void Test02TrackedEntitiesUpdateOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var description = db.ProductDescriptions.OrderByDescending(x => x.ProductDescriptionID).First();
                var originalGuid = description.rowguid;

                //ATTEMPT
                description.Description = Guid.NewGuid().ToString();
                var status = db.SaveChangesWithChecking();

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                description.ModifiedDate.Ticks.ShouldEqualWithTolerance(DateTime.UtcNow.Ticks, HalfSecondInTicks);
                description.rowguid.ShouldEqual(originalGuid);
            }
        }

    }
}

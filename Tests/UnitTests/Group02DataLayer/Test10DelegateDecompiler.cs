#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test02CustomerList.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Linq;
using System.Reflection;
using DataLayer.GeneratedEf;
using DelegateDecompiler;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.UnitTests.Group02DataLayer
{
    class Test10DelegateDecompiler
    {

        [Test]
        public void Test01CustomerNormalFullNameOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var list = db.Customers.Select(c => c.Title + " " + c.FirstName + " " + c.LastName + " " + c.Suffix).ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
            }
        }

        [Test]
        public void Test02CustomerWithComputedFullNameOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var list = db.Customers.Select(x => x.FullName).Decompile().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
            }
        }

        [Test]
        public void Test05CustomerWithComputedHasSalesOrderOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var list = db.Customers.Where(x => x.HasBoughtBefore).Decompile().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
                list.Count.ShouldBeLessThan(100);
            }
        }

        [Test]
        public void Test10AddressWithComputedFullAddressOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var list = db.Addresses.Select(x => x.FullAddress).Decompile().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
                string.IsNullOrEmpty( list.First()).ShouldEqual(false);
                list.Any( x => x.Contains(", ,")).ShouldEqual(false);
            }
        }



        //[Test]
        //public void Test01CustomerWithComputedFullNameSingleOk()
        //{
        //    using (var db = new AdventureWorksLt2012())
        //    {
        //        //SETUP
        //        var firstId = db.Customers.Select(x => x.CustomerID).First();

        //        //ATTEMPT
        //        var any = db.Customers.Any( y => y.FullName == "Test User");

        //        //VERIFY
        //        any.ShouldEqual(false);
        //    }
        //}


        [Test]
        [Ignore("Timing test.")]
        public void Test10TimingDifferenceOk()
        {
            const int NumTimes = 1000;
            const int NumTakes = 10;
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var firstId = db.Customers.Select(x => x.CustomerID).First();


                //ATTEMPT
                TimedDelegateDecompiler(NumTimes, db, NumTakes);
                TimedCheckIfItNeedsDecompile(NumTimes, db, NumTakes, false);
                TimedCheckIfItNeedsDecompile(NumTimes, db, NumTakes, true);
                TimedCheckIfItNeedsDecompile(NumTimes, db, NumTakes, false);
                TimedNormalAccess(NumTimes, db, NumTakes);
                TimedNormalAccessButCallDecompileAnyway(NumTimes, db, NumTakes);
                TimedDelegateDecompiler(NumTimes, db, NumTakes);
                TimedNormalAccess(NumTimes, db, NumTakes);
                TimedNormalAccessButCallDecompileAnyway(NumTimes, db, NumTakes);
                TimedDelegateDecompiler(NumTimes, db, NumTakes);

                //VERIFY

            }
        }

        [Test]
        public void Test11TimimgScanClassOk()
        {
            const int NumTimes = 1000;
            using (new TimerToConsole("Initial scan", NumTimes))
                for (int i = 0; i < NumTimes; i++)
            {
                var addDecompile = typeof (Customer).GetCustomAttribute<ComputedAttribute>() != null;
            }
            using (new TimerToConsole("Another scan", NumTimes))
                for (int i = 0; i < NumTimes; i++)
            {
                var addDecompile = typeof(Customer).GetCustomAttribute<ComputedAttribute>() != null;
            }

        }


        private static void TimedCheckIfItNeedsDecompile(int NumTimes, AdventureWorksLt2012 db, int NumTakes, bool okToUseDecompileIfNeeded)
        {

            using (var timer = new TimerToConsole("Normal access, but checks if needed. Decompile " + (okToUseDecompileIfNeeded ? "was added" : "was NOT added"), NumTimes))
            {
                var addDecompile = typeof (Customer).GetCustomAttribute<ComputedAttribute>() != null &&
                                   okToUseDecompileIfNeeded;
                for (int i = 0; i < NumTimes; i++)
                {
                    var expression = db.Customers.Select(
                        c => c.Title + " " + c.FirstName + " " + c.LastName + " " + c.Suffix)
                        .Take(NumTakes);

                    var item1 = addDecompile ? expression.Decompile().ToList() : expression.ToList();
                }
            }
        }

        private static void TimedNormalAccessButCallDecompileAnyway(int NumTimes, AdventureWorksLt2012 db, int NumTakes)
        {
            using (var timer = new TimerToConsole("Normal access, but calling decompile anyway", NumTimes))
                for (int i = 0; i < NumTimes; i++)
                {
                    var item1 =
                        db.Customers.Select(c => c.Title + " " + c.FirstName + " " + c.LastName + " " + c.Suffix)
                            .Take(NumTakes)
                            .Decompile()
                            .ToList();
                }
        }


        private static void TimedDelegateDecompiler(int NumTimes, AdventureWorksLt2012 db, int NumTakes)
        {
            using (var timer = new TimerToConsole("With DelegateDecompiler", NumTimes))
                for (int i = 0; i < NumTimes; i++)
                {
                    var item1 =
                        db.Customers.Select(x => x.FullName).Take(NumTakes).Decompile().ToList();
                }
        }

        private static void TimedNormalAccess(int NumTimes, AdventureWorksLt2012 db, int NumTakes)
        {
            using (var timer = new TimerToConsole("Normal access", NumTimes))
                for (int i = 0; i < NumTimes; i++)
                {
                    var item1 =
                        db.Customers.Select(c => c.Title + " " + c.FirstName + " " + c.LastName + " " + c.Suffix)
                            .Take(NumTakes)
                            .ToList();
                }
        }
    }
}

#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test03UiClasses.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Collections.Generic;
using NUnit.Framework;
using ServiceLayer.UiClasses;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test03UiClasses
    {

        [Test]
        public void Test01DropDownEmptyOk()
        {
            //SETUP
            var ddl = new DropDownListType();

            //ATTEMPT
            ddl.SetupDropDownListContent(new KeyValuePair<string, string>[] {}, "There is a prompt");


            //VERIFY
            ddl.SelectedValueAsInt.ShouldEqual(null);
        }


    }
}

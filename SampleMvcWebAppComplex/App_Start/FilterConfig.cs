#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: FilterConfig.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Web.Mvc;

namespace SampleMvcWebAppComplex
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

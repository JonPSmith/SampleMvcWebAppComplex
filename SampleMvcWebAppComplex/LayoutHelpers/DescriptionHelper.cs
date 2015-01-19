#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: DescriptionHelper.cs
// Date Created: 2014/10/22
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace SampleMvcWebAppComplex.LayoutHelpers
{
    public static class DescriptionHelper
    {
        /// <summary>
        /// This is an html helper that adds tooltips using the DisplayAttribute. 
        /// It is used like Html.EditorFor etc.
        /// See Views\Customer\Create.cshtml and properties Suffix and Phone for how it is used
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="helper"></param>
        /// <param name="source"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MvcHtmlString DescriptionToToolTip<T,TV>(this HtmlHelper helper, T source, Expression<Func<T,TV>> model) where T : class
        {
            var memberEx = (MemberExpression)model.Body;
            if (memberEx == null)
                throw new ArgumentNullException("model", "You must supply a LINQ expression that is a property.");

            var propInfo = typeof(T).GetProperty(memberEx.Member.Name);
            if (propInfo == null)
                throw new ArgumentNullException("model", "The member you gave is not a property.");

            var displayAttr = propInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttr == null || string.IsNullOrEmpty(displayAttr.GetDescription())) return null;

            //it outputs a tooltop
            return new MvcHtmlString("<a href='#' class='k-icon k-i-note' title='" + displayAttr.GetDescription() + "' id='" + memberEx.Member.Name + "'>?</a>");
        }
    }
}
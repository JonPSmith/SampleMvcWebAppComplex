#region licence

// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: DiModelBinder.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================

#endregion

using System;
using System.Web.Mvc;

namespace SampleMvcWebAppComplex.Infrastructure
{
    public class DiModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext,
            Type modelType)
        {
            return modelType.IsInterface
                ? DependencyResolver.Current.GetService(modelType)
                : base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}
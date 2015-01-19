#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ServiceTypeClass.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;

namespace Tests.DependencyItems
{
    class ServiceTypeClass
    {
        private Type _resolvedType;

        public ServiceTypeClass(Type resolvedType)
        {
            this._resolvedType = resolvedType;
        }
    }
}

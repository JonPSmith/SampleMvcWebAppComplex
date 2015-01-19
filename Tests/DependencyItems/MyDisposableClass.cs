#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: MyDisposableClass.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;

namespace Tests.DependencyItems
{
    public interface IMyDisposableClass { }

    public class MyDisposableClass : IMyDisposableClass, IDisposable
    {

        private readonly Action _disposeWasCalled;

        public MyDisposableClass(Action disposeWasCalled)
        {
            _disposeWasCalled = disposeWasCalled;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _disposeWasCalled();
                }
            }
            _disposed = true;
        }
        
    }
}

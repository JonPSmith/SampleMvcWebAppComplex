#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: GenericInterface.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion
namespace Tests.DependencyItems
{
    public interface IGenericInterface<T> where T : class
    {
        string GetTypeName();
    }

    public class GenericInterface<T> : IGenericInterface<T> where T : class
    {

        public string GetTypeName()
        {
            return typeof (T).Name;
        }

    }
}

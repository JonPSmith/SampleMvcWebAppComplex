#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: SimpleClass.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion
namespace Tests.DependencyItems
{
    public interface ISimpleClass
    {
        void DoSomething();
    };

    public class SimpleClass : ISimpleClass
    {
        public void DoSomething()
        {
        }
    }
}

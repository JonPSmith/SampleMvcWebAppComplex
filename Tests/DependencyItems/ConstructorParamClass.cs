#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ConstructorParamClass.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion
namespace Tests.DependencyItems
{
    public interface IConstructorParamClass
    {
        int MyInt { get; }
    }

    public class ConstructorParamClass : IConstructorParamClass
    {

        public int MyInt { get; private set; }

        public ConstructorParamClass(int myInt)
        {
            MyInt = myInt;
        }
    }
}

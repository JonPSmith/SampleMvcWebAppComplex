using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.UiClasses
{
    /// <summary>
    /// This class is used to display some text associated to some Text
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KeyTextClass<T>
    {

        public T Key { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Text, Key);
        }

        public KeyTextClass() { } 

        public KeyTextClass(T key, string text)
        {
            Key = key;
            Text = text;
        }
    }
}

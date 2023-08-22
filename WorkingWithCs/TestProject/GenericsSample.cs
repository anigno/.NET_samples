using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public abstract class SumabaleBase<T>
    {
        public abstract T Sum(T t1, T t2);
    }



    internal class GenericsSampleClass<T> : SumabaleBase<T> where T : SumabaleBase<T>, new()
    {
        public T item = new T();

        public GenericsSampleClass(T t)
        {
            item = t;
        }

        public override T Sum(T t1, T t2)
        {
            throw new NotImplementedException();
        }
    }

    internal class SomeClass
    {
        public static T Add<T>(T num1, T num2) where T : IConvertible
        {
            return (T)((dynamic)num1 + (dynamic)num2);
        }

        public bool Compare<T>(T item1, T item2) where T : IComparable
        {
            return item1.CompareTo(item2) == 0;
        }
    }


}

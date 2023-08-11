using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopExample
{

    public interface ISpecialNumber
    {
        ISpecialNumber Add(ISpecialNumber specialNumber1, ISpecialNumber specialNumber2);
    }

    public abstract class SpecialNumber : ISpecialNumber
    {
        public static ISpecialNumber operator +(SpecialNumber num1, ISpecialNumber num2)
        {
            return num1.Add(num1, num2);
        }

        public abstract ISpecialNumber Add(ISpecialNumber specialNumber1, ISpecialNumber specialNumber2);
    }

}

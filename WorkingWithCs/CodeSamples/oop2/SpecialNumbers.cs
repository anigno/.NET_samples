using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OopExample
{

    public class RationalNumber : SpecialNumber
    {
        public RationalNumber(int counter, int devider)
        {
            Counter = counter;
            Devider = devider;
        }

        public RationalNumber(RationalNumber rationalNumber) :
            this(rationalNumber.Counter, rationalNumber.Devider)
        {
        }

        public int Counter { get; set; }
        public int Devider { get; set; }

        public override ISpecialNumber Add(ISpecialNumber specialNumber1, ISpecialNumber specialNumber2)
        {
            RationalNumber r1 = specialNumber1 as RationalNumber;
            RationalNumber r2 = specialNumber2 as RationalNumber;
            int d = r1.Devider * r2.Devider;
            int c = d / r1.Devider * r1.Counter + d / r2.Devider * r2.Counter;
            return new RationalNumber(c, d);
        }


        public override string ToString()
        {
            return $"({Counter}/{Devider})";
        }
    }

}

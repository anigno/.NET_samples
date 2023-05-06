#region

using System;
using BlCommon;

#endregion

namespace MyMathServiceLib
{
    public class MyMathService : IMyMathService
    {
        #region Constructors

        public MyMathService()
        {
            TestUtils.ConsoleLog(ConsoleColor.Cyan, "MyMathService constructor");
        }

        #endregion

        #region Public Methods

        public double Add(double p_firstNnumber, double p_SecondNumber)
        {
            TestUtils.ConsoleLog(ConsoleColor.Yellow, "Add({0},{1})", p_firstNnumber, p_SecondNumber);
            return p_firstNnumber + p_SecondNumber;
        }

        public double Substract(double p_firstNnumber, double p_SecondNumber)
        {
            TestUtils.ConsoleLog(ConsoleColor.Yellow, "Substract({0},{1})", p_firstNnumber, p_SecondNumber);
            return p_firstNnumber - p_SecondNumber;
        }

        #endregion

        #region Fields


        #endregion
    }
}
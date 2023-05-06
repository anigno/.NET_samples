#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace PropertiesGetSetValues
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            TestClassA classA = new TestClassA() {IntegerProperty = 17, StringProperty = "Hello", DoublesArrayProperty = new[] {1.23, 4.567}};
            TestClassB classB = new TestClassB();
            PropertyReflection.CopyProperties(classA, classB);
        }

        #endregion
    }

    public class TestClassBase
    {
        #region Constructors

        public TestClassBase()
        {
            IntegerProperty = 0;
            DoublesArrayProperty = new[] {0.0, 0.0};
        }

        #endregion

        #region Public Properties

        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; }
        public double[] DoublesArrayProperty { get; set; }

        #endregion
    }

    public class TestClassA : TestClassBase
    {
    }

    public class TestClassB : TestClassBase
    {
    }
}
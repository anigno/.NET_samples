using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.ResourcesTest
{
    [TestFixture]
    public class ResourceTesting
    {
        [Test]
        public void Test()
        {
            Console.WriteLine("CurrentUICulture=[{0}]", Thread.CurrentThread.CurrentUICulture);
            string s1 = MyResource.ResourceOne;
            Console.WriteLine("Resource value=[{0}]", s1);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("he-IL");
            Console.WriteLine("CurrentUICulture=[{0}]", Thread.CurrentThread.CurrentUICulture);
            string s2 = MyResource.ResourceOne;
            Console.WriteLine("Resource value=[{0}]", s2);
            Assert.AreNotEqual(s1,s2);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("");
            Console.WriteLine("CurrentUICulture=[{0}]", Thread.CurrentThread.CurrentUICulture);
            string s3 = MyResource.ResourceOne;
            Console.WriteLine("Resource value=[{0}]", s3);
            Assert.AreEqual(s1, s3);
        }
    }
}

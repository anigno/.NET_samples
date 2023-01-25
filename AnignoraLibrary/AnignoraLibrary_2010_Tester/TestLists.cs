using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnignoraDataTypes.Lists;
using NUnit.Framework;
using AnignoraDataTypes;

namespace AnignoraLibrary_2010_Tester
{
    [TestFixture]
    public class TestLists
    {
        private class TestClass
        {
            public int IntegerVar;
        }

        [Test]
        public void TestListFiltered()
        {
            ListFiltered<TestClass> list = new ListFiltered<TestClass>(p_class => p_class.IntegerVar < 5);
            for (int a = 0; a < 10; a++)
            {
                list.AddFiltered(new TestClass() { IntegerVar = a });
            }
            Assert.AreEqual(5,list.Count);
            list.DoForAll(p_class => { p_class.IntegerVar += 3; });
            TestClass[] items=list.FilterExistingItems();
            Assert.AreEqual(3,items.Length);
            Assert.AreEqual(2,list.Count);
        }
    }
}

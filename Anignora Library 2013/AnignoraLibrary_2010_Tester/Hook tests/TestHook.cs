using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnignoraCommonAndHelpers.HooksAndImmulate;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.Hook_tests
{
    [TestFixture]
    public class TestHook
    {
        [Test]
        [Ignore]
        public void TestHooks()
        {
            FormHookTester f=new FormHookTester();
            Application.Run(f);
        }
    }
}

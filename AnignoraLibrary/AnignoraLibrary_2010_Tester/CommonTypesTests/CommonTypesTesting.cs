using System;
using AnignoraDataTypes.CommonTypes;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester.CommonTypesTests
{
    [TestFixture]
    public class CommonTypesTesting
    {
        [Test]
        public void TestTime()
        {
            Time t1=new Time();
            Time t2=new Time(new DateTime(1,1,1,0,0,0,0));
            Time t3=new Time(t1);
            Time t4=new Time(0,0,0,0);
            Assert.True(t1==t2);
            Assert.True(t3==t4);
            Assert.True(t1==t4);

            t1=new Time(1,1,1,1);
            t2=new Time(2,2,2,2);
            t3 = t1 + t2;
            t4 = t3 - t1;
            Assert.AreEqual(t4,t2);

            t1=new Time(0,0,0,-1);
            t2=new Time(23,59,59,999);
            Assert.AreEqual(t1,t2);

            t1=new Time(2,0,0,1);
            t2=new Time(2,0,0,0);
            Assert.True(t1>t2);

            t1=new Time(0,0,0,9999999);
            TimeSpan ts = TimeSpan.FromMilliseconds(9999999);
            Assert.AreEqual(t1.Hour,ts.Hours);
            Assert.AreEqual(t1.Minute, ts.Minutes);
            Assert.AreEqual(t1.Second, ts.Seconds);
            Assert.AreEqual(t1.Millisecond, ts.Milliseconds);
        }
    }
}

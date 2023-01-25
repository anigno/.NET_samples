using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AnignoraCommunication;
using AnignoraCommunication.Communicators;
using NUnit.Framework;
using AnignoraCommonAndHelpers;

namespace AnignoraLibrary_2010_Tester
{
    [TestFixture]
    public class TestCrc32
    {
        [Test]
        public void TestCrc()
        {
            UTF32Encoding enc = new UTF32Encoding();
            HashSet<uint> set=new HashSet<uint>();
            const int numberOfTests = 1000000;
            int duplicatesCnt = 0;
            //Check duplicates crc numbers for strings of numbers
            for (int a = 0; a < numberOfTests; a++)
            {
                byte[] bytes = enc.GetBytes(a.ToString());
                uint crc = Crc32.CalculateCrc32Uint(bytes);
                if (set.Contains(crc))duplicatesCnt++;
                set.Add(crc);
            }
            StringBuilder sb=new StringBuilder();
            //Check duplicates crc numbers for long strings
            for (int a = 0; a < 10000; a++)
            {
                sb.Append("A");
                byte[] bytes = enc.GetBytes(sb.ToString());
                uint crc = Crc32.CalculateCrc32Uint(bytes);
                if (set.Contains(crc)) duplicatesCnt++;
                set.Add(crc);
            }
            float crcGradePer=100f-100f*duplicatesCnt/numberOfTests;
            Debug.WriteLine("CRC duplicates: {0}/{1} CRC Grade: {2}", duplicatesCnt,numberOfTests,crcGradePer);
            Assert.GreaterOrEqual(crcGradePer,99.99f);
            byte[] bs =new byte[] {100,101 };
            bool b=Crc32.AreEqual(Crc32.CalculateCrc32(bs), Crc32.CalculateCrc32(bs));
            Assert.True(b);
        }
    }
}

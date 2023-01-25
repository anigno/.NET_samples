using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BlCommon
{
    [TestFixture]
    class LinqExamples
    {
        [Test]
        public void ChangeTypeWithSelectToSpecificType()
        {
            IEnumerable<KeyValuePair<int, string>> processesIds = Process.GetProcesses().Select(process => new KeyValuePair<int, string>(process.Id, process.ProcessName));
            Debug.WriteLine(processesIds.Count());
        }

        [Test]
        public void ChangeTypeWithSelectToAnonimousType()
        {
            var processesIds = Process.GetProcesses().Select(process => new { process.Id, process.ProcessName });
            Debug.WriteLine(processesIds.Count());
        }

        [Test]
        public void TestAggrigate01()
        {
            List<int> numbers = new List<int> { 6, 2, 8, 3 };
            int sum1 = numbers.Aggregate((result, item) => result + item);
            // sum: (((6+2)+8)+3) = 19
            Debug.WriteLine(sum1);
        }

        [Test]
        public void TestAggrigate02()
        {
            double[] numbers = { 6, 2, 8, 3 };
            double sum1 = numbers.Aggregate(0.0, (result, item) => { return result + item; }, i => i / numbers.Length);
            Debug.WriteLine(sum1);
        }

        [Test]
        public void TestAggrigate03()
        {
            double[] numbers = { 6, 2, 8, 3 };
            const double INIT_VALUE = 0;
            double sum1 = numbers.Aggregate(INIT_VALUE, IterationMethodUsesAccumolatorAndItem, PostAccumolatiopnActivity);
            Debug.WriteLine(sum1);
        }

        private double PostAccumolatiopnActivity(double d)
        {
            return d / 4;
        }

        private double IterationMethodUsesAccumolatorAndItem(double Acumolator, double item)
        {
            return Acumolator + item;
        }

        [Test]
        public void TestAggrigate04()
        {
            string[] letters = new[] { "A", "B", "C", "D" };
            var sum1 = letters.Aggregate(new List<string>(), ConcatFunc, ResultSelector);
            Debug.WriteLine(sum1);
        }

        private string ResultSelector(List<string> list)
        {
            return list.Aggregate("", (current, s) => current + s);
        }

        private List<string> ConcatFunc(List<string> list, string s)
        {
            list.Add(s);
            return list;
        }
    }
}

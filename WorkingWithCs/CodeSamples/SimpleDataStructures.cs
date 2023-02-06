using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeSamples
{
    internal class SimpleDataStructures
    {
        public static void PrintKeyValues(IEnumerable<KeyValuePair<int, int>> enumerableKeyValues, string comment = "")
        {

            foreach (KeyValuePair<int, int> kvp in enumerableKeyValues) Console.Write($"{kvp.Key}:{kvp.Value} ");
            Console.Write($" :{comment}");
            Console.WriteLine();
        }

        public static void PrintEnumerable(IEnumerable enumerable, string comment = "")
        {
            foreach (object o in enumerable)
            {
                Console.Write($"{o.ToString()} ");
             }
            Console.Write($" :{comment}");
            Console.WriteLine();

        }

        public void TestDictionary()
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(1, 10);
            try { dict.Add(1, 0); } catch (Exception) { Console.WriteLine("can't add existing key"); }
            dict[2] = 20;
            bool b = dict.TryAdd(3, 30); // add
            PrintKeyValues(dict, "added values");
            b = dict.TryAdd(3, 31); // already exist, will not update
            dict[2] = 21; //add
            PrintKeyValues(dict, $"TryAdd 3 ignored [returned {b}], 2 is updated");
            dict.Remove(3);
            PrintKeyValues(dict, "removed 3");
            dict[7] = 70;
            dict[5] = 50;
            PrintKeyValues(dict, "unordered add");
            Console.WriteLine("dict contains 2 " + dict.ContainsKey(2));
            Console.WriteLine("dict count " + dict.Count);
        }

        public void TestSortedDictionary()
        {
            SortedDictionary<int, int> sdict = new SortedDictionary<int, int>();
            sdict[5] = 50;
            sdict[4] = 40;
            sdict[6] = 60;
            PrintKeyValues(sdict, "added 5,4,6");
        }

        class ComparerInt : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y - x;
            }
        }
        public void TestSortedList()
        {
            SortedList<int, int> slist = new SortedList<int, int>(new ComparerInt());
            slist[2] = 20;
            slist[1] = 10;
            slist.Add(3, 30);
            PrintKeyValues(slist, "added 2,1,3 using custom comparer");
        }

        public static void TestStackAndQueueAndHashSet()
        {
            var stk = new Stack<int>();
            var que = new Queue<int>();
            for (int a = 0; a < 5; a++)
            {
                stk.Push(a);
                que.Enqueue(a);
            }
            Console.WriteLine("enter 0->5");
            PrintEnumerable(stk,"stack");
            PrintEnumerable(que,"queue");
            Console.WriteLine($"{stk.ElementAt(3)} stk element at 3");
            Console.WriteLine($"{que.ElementAt(3)} que element at 3");
            Console.WriteLine("stk contains 4 "+stk.Contains(4));
            

            var set=new HashSet<int>();
            set.Add(3);
            set.Add(4);
            set.Add(4);
            PrintEnumerable(set, "set of 3,4,4");

        }
    }
}

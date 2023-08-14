using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples
{
    internal class MoreOnDataStructures
    {
        public MoreOnDataStructures()
        {
            var queue = new Queue<int>();
            var stack = new Stack<int>();
            var set = new HashSet<int>();
            var heap = new PriorityQueue<int, int>();
            var list = new List<int>();
            var sortedList = new SortedList<int, int>();    //array based
            var sortedDictionary = new SortedDictionary<int, int>();    //tree based
        }

    }
}

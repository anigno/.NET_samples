using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LeatcodeQuestions
{
    [TestClass]
    public class Group01
    {
        #region IsStr1InStr2
        [TestMethod]
        public void TestIsStr1InStr2()
        {
            Assert.AreEqual(1, IsStr1InStr2(new char[] { '1', '2', '\0' }, new char[] { '1', '1', '2', '\0' }));
            Assert.AreEqual(-1, IsStr1InStr2(new char[] { '1', '2', '\0' }, new char[] { '1', '3', '2', '\0' }));
            Assert.AreEqual(0, IsStr1InStr2(new char[] { '1', '\0' }, new char[] { '1', '\0' }));
        }

        //find chars1 in chars2
        static int IsStr1InStr2(char[] chars1, char[] chars2)
        {
            int a = 0;
            int b = 0;
            int b1 = 0;
            while (chars1[a] != 0 && chars2[b] != 0)
            {
                while (chars1[a] == chars2[b])
                {
                    a++;
                    b++;
                    if (chars1[a] == 0)
                        return b1;
                }
                b1++;
                b = b1;
                a = 0;
            }
            return -1;
        }

        #endregion

        #region GenerateParenthesis
        [TestMethod]
        public void TestGenerateParenthesis()
        {
            Assert.AreEqual(2, GenerateParenthesis(2).Count);
            Assert.AreEqual(5, GenerateParenthesis(3).Count);
        }

        //generate all posible parenthesis for string of i*2 
        static List<string> GenerateParenthesis(int nPairs)
        {
            Trace.WriteLine(nPairs);
            List<string> ret = new List<string>();
            GenerateRecurse(ret, "", nPairs, 0, 0);
            return ret;
        }

        static void GenerateRecurse(List<string> result, string currentPars, int nPairs, int nPlaced, int sumLegal)
        {
            if (nPlaced > nPairs * 2 || sumLegal < 0 || sumLegal > nPairs)
                return;
            if (sumLegal == 0 && nPlaced == nPairs * 2)
            {
                Trace.WriteLine(currentPars);
                result.Add(currentPars);
                return;
            }
            GenerateRecurse(result, currentPars + "(", nPairs, nPlaced + 1, sumLegal + 1);
            GenerateRecurse(result, currentPars + ")", nPairs, nPlaced + 1, sumLegal - 1);
        }
        #endregion

        #region FindShortestContainingString

        [TestMethod]
        public void TestFindShortestContainingString()
        {
            //(ABCA) FFDDADCFEBE CEABEBA DFCDFCDFCBFCBEAD
            int[] result = FindShortestContainingString("ABCA", "FFDDADCFEBECEABEBADFCDFCDFCBFCBEAD");
            Assert.AreEqual(7, result[0]);  //length
            Assert.AreEqual(11, result[1]); //index
        }

        //look for shortest source string in target that containt all chars from source
        private int[] FindShortestContainingString(string source, string target)
        {
            char[] src = source.ToCharArray();
            char[] tar = target.ToCharArray();
            int start = 0;
            int end = src.Length - 1;
            int minFound = tar.Length + 1;
            int minStartIndex = -1;
            Dictionary<char, int> requestedCounts = new Dictionary<char, int>();
            Dictionary<char, int> currentCounters = new Dictionary<char, int>();
            requestedCounts = CountChars(src, 0, end);
            currentCounters = CountChars(tar, start, end);
            Dictionary<char, int> counters = new Dictionary<char, int>();
            if (IsSatisfied(requestedCounts, currentCounters)) return new int[] { 0, end + 1 };
            while (end < tar.Length - 1)
            {
                //add char at end
                end++;
                currentCounters = CountChars(tar, start, end);
                if (IsSatisfied(requestedCounts, currentCounters))
                {
                    //try to remove chars at start
                    do
                    {
                        int currentMin = end - start + 1;
                        if (currentMin < minFound)
                        {
                            minFound = currentMin;
                            minStartIndex = start;
                        }
                        currentCounters[tar[start]]--;
                        start++;
                    } while (IsSatisfied(requestedCounts, currentCounters));

                }

            }
            if (minFound < tar.Length + 1) return new int[] { minFound, minStartIndex };
            return new int[] { -1, -1 };
        }

        //verify that current counted dictionary contains atleast requested count of each char
        public bool IsSatisfied(Dictionary<char, int> requested, Dictionary<char, int> current)
        {
            foreach (char c in requested.Keys)
            {
                if (!current.ContainsKey(c)) return false;
                if (current[c] < requested[c]) return false;
            }
            return true;
        }


        //count number of occurences of chars in chars and return dictionary with results
        private Dictionary<char, int> CountChars(char[] chars, int start, int end)
        {
            Dictionary<char, int> counters = new Dictionary<char, int>();
            for (int i = start; i <= end; i++)
            {
                char c = chars[i];
                if (!counters.ContainsKey(c)) counters[c] = 0;
                counters[c]++;
            }
            return counters;
        }

        #endregion
    }
}


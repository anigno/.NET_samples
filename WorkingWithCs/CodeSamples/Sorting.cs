using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeSamples
{
    public class ListNode
    {
        public int val = 0;
        public ListNode? next = null;
    }

    internal class Sorting
    {
        public static ListNode BuildLinkedList(int[] vals)
        {
            ListNode head = new ListNode();
            head.val = vals[0];
            ListNode iNode = head;
            foreach (int i in vals.Skip(1))
            {
                ListNode node = new ListNode();
                node.val = i;
                iNode.next = node;
                iNode = node;
            }
            return head;
        }

        public ListNode InsertSort(ListNode head)
        {
            for (ListNode? i = head; i != null; i = i.next)
            {
                for (ListNode? j = head; j != null; j = j.next)
                {
                    if (i.val < j.val)
                    {
                        replaceVals(i, j);
                    }
                }
            }
            return head;
        }

        public ListNode MergeSort(ListNode head)
        {
            int length = calcLength(head);
            if (length > 2)
            {
                ListNode? right, left;
                splitLIstNodes(head, out left, out right);
                left = MergeSort(left);
                right = MergeSort(right);
                ListNode mergedNode = MergeLists(left, right);
                return mergedNode;
            }
            PrintListNodes(head, "<=2");
            if (length > 1)
            {
                if (head.val > head.next.val)
                {
                    replaceVals(head, head.next);
                }
            }
            return head;
        }

        private ListNode MergeLists(ListNode left, ListNode right)
        {
            ListNode? iNode = null;
            if (left.val < right.val)
            {
                iNode= left;
                left = left.next;
            }
            else
            {
                iNode= right;
                right=right.next;
            }
            ListNode? mergedNode = iNode;

            while (left != null || right != null)
            {
                if (left == null)
                {
                    iNode.next = right;
                    iNode = iNode.next;
                    right = right.next;
                }
                else if(right==null || left.val < right.val)
                {
                    iNode.next = left;
                    iNode = iNode.next;
                    left = left.next;
                }
                else 
                {
                    iNode.next = right;
                    iNode = iNode.next;
                    right = right.next;
                }
            }
            return mergedNode;
        }

        private int calcLength(ListNode node)
        {
            if (node == null) return 0;
            int a = 0;
            for (ListNode? i = node; i != null; i = i.next, a++) ;
            return a;
        }

        private void splitLIstNodes(ListNode node, out ListNode? left, out ListNode? right)
        {
            int length = calcLength(node);
            int cnt = 0;
            ListNode? iNode = node;
            ListNode? prevNode = node;
            for (; cnt < length / 2; prevNode = iNode, iNode = iNode.next, cnt++) ;
            left = node;
            right = iNode;
            prevNode.next = null;
        }

        private static void replaceVals(ListNode i, ListNode j)
        {
            (j.val, i.val) = (i.val, j.val);
        }

        public static void PrintListNodes(ListNode head, string comment = "no comment")
        {
            for (ListNode? i = head; i != null; i = i.next)
            {
                Console.Write($"{i.val} ");
            }
            Console.WriteLine(comment);
        }
    }
}

/*
4321

*/
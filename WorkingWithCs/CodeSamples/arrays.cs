namespace CodeSamples
{
    public class ArraysSamples
    {
        public ArraysSamples()
        {
            //two dim arrays
            int[,] a = new int[2, 3];
            int[,] b = { { 1, 2, 3 }, { 4, 5, 6 } };
            foreach (int i in a) Console.Write($"{i}");
            Console.WriteLine();
            foreach (int i in b) Console.Write($"{i}");
            Console.WriteLine();

            //list of arrays
            List<int[]> listOfArrays = new();
            listOfArrays.Add(new int[10]);
            listOfArrays.Add(new int[10]);
            listOfArrays.Add(new int[10]);
            listOfArrays[2][7] = 5;
            Console.WriteLine(listOfArrays[2][7]);

            //array of refs to arrays
            int[][] arrayOfArrays = new int[3][];
            arrayOfArrays[0] = new int[10];
            arrayOfArrays[1] = new int[10];
            arrayOfArrays[2] = new int[10];
            arrayOfArrays[2][5] = 17;
            Console.WriteLine(arrayOfArrays[2][5]);

            int[] fillArray = new int[10];
            Array.Fill(fillArray, 5, 2, 3);
            foreach (int i in fillArray) Console.Write($"{i}");
            Console.WriteLine();

            int[] rangeArray = Enumerable.Range(3, 9 - 3 + 1).ToArray();
            foreach (int i in rangeArray) Console.Write($"{i}");
            Console.WriteLine();

            int[] skipTakeArray = rangeArray.Skip(2).Take(3).ToArray();
            foreach (int i in skipTakeArray) Console.Write($"{i}");
            Console.WriteLine();

            int[] ints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            ArraySegment<int> arrySeg = new ArraySegment<int>(ints, 2, 3);
            arrySeg[0] = 99;
            for (int q = 0; q < ints.Length; q++)
                Console.Write($"{q}:{ints[q]} ");
            Console.WriteLine();
            for (int q = 0; q < arrySeg.Count; q++)
                Console.Write($"{q}:{arrySeg[q]} ");
            Console.WriteLine();
            int[] newArry=new int[3];
            Array.Copy(ints, 2, newArry, 0, 3);
            newArry[0] = 55;
            for (int q = 0; q < ints.Length; q++)
                Console.Write($"{q}:{ints[q]} ");
            Console.WriteLine();
            for (int q = 0; q < newArry.Length; q++)
                Console.Write($"{q}:{newArry[q]} ");
            Console.WriteLine();

        }
    }
}
namespace HWUnity
{
    internal class Program
    {
        private static void Main()
        {
            //Console.WriteLine(Task1(7, 21));

            //Console.WriteLine(Task2([81, 22, 13, 54, 10, 34, 15, 26, 71, 68]));

            //Console.WriteLine(Task3([81, 22, 13, 34, 10, 34, 15, 26, 71, 68], 34));
            //Console.WriteLine(Task3([81, 22, 13, 34, 10, 34, 15, 26, 71, 68], 55));

            //var result = Task4([1, 5, 7, 8, 0, 4, 4, 7, 8]);
            //foreach (var el in result)
            //{
            //    Console.WriteLine(el);
            //}
        }

        private static int Task1(in int min, in int max)
        {
            var sum = 0;
            for (var i = min; i <= max; i++)
            {
                if (i % 2 == 0) sum += i;
            }

            return sum;
        }

        private static int Task2(IEnumerable<int> arr)
        {
            return arr.Where(el => el % 2 == 0).Sum();
        }

        private static int Task3(IReadOnlyCollection<int> arr, int num)
        {
            for (var i = 0; i < arr.Count; i++)
            {
                if (arr.ElementAt(i) == num) return i;
            }

            return -1;
        }

        private static int[] Task4(int[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                var min = arr[i];
                var minIndex = i;
                for (var j = i; j < arr.Length; j++)
                {
                    if (arr[j] >= min) continue;
                    min = arr[j];
                    minIndex = j;
                }
                (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
            }
            return arr;
        }
    }
}

namespace HW5
{
    internal class Program
    {
        private static void Main()
        {
            Task2.TaskMain();
        }
    }

    public static class Task1
    {
        public static void TaskMain()
        {
            Console.WriteLine("Enter your text:");
            var text = Console.ReadLine();
            var words = SplitText(text ?? string.Empty);
            PrintWords(words);
        }

        public static IEnumerable<string> SplitText(string text) => text.Split(' ');

        public static void PrintWords(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                Console.WriteLine(word);
            }
        }
    }

    public static class Task2
    {

        public static void TaskMain()
        {
            Console.WriteLine("Enter your text:");
            var text = Console.ReadLine();
            Console.WriteLine(Reverse(text ?? string.Empty));
        }

        public static string Reverse(string? text)
        {
            var words = SplitText(text);
            var len = words.Length;

            for (var i = 0; i < len / 2; i++)
            {
                (words[len - i - 1], words[i]) = (words[i], words[len - i - 1]);
            }

            return string.Join(" ", words);
        }

        public static string[] SplitText(string? text) => text.Split(' ');
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static async Task Print(string text, int waitTimeSecs, string threadName)
    {
        while (true)
        {
            Console.WriteLine(threadName + ": " + text);
            await Task.Delay(waitTimeSecs * 1000);
        }
    }

    private static async Task Main()
    {
        var task1 = Print("async Print #1", 2, "Thread 1");
        var task2 = Print("async Print #2", 3, "Thread 2");
        var task3 = Print("async Print #3", 5, "Thread 3");

        await Task.WhenAll(task1, task2, task3);

    }
}
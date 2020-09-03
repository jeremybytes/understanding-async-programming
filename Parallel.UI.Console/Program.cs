using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Parallel.UI
{
    class Program
    {
        private static CancellationTokenSource tokenSource;

        static void Main(string[] args)
        {
            using (tokenSource = new CancellationTokenSource())
            {
                Console.Clear();
                Console.WriteLine("Please make a selection:");
                Console.WriteLine("1) Use Task (parallel)");
                Console.WriteLine("2) Use await (non-parallel)");
                Console.WriteLine("(any other key to exit)");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine();
                        Task.Run(() => UseTask(tokenSource.Token));
                        HandleExit();
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine();
                        Task.Run(() => UseAwait(tokenSource.Token));
                        HandleExit();
                        break;
                    default:
                        Console.WriteLine();
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static async Task UseTask(CancellationToken cancelToken)
        {
            Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");

            var reader = new PersonReader();
            try
            {
                List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

                var taskList = new List<Task>();

                foreach (int id in ids)
                {
                    Task<Person> personTask = reader.GetPersonAsync(id, tokenSource.Token);
                    taskList.Add(personTask);

                    _ = personTask.ContinueWith(task =>
                    {
                        Person person = task.Result;
                        Console.WriteLine(person.ToString());
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                }

                await Task.WhenAll(taskList).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nThe operation was canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere was a problem retrieving data");
                Console.WriteLine($"\nERROR\n{ex.GetType()}\n{ex.Message}");
                Environment.Exit(1);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private async static Task UseAwait(CancellationToken cancelToken)
        {
            Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");

            var reader = new PersonReader();

            try
            {
                List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

                foreach (int id in ids)
                {
                    Console.WriteLine(await reader.GetPersonAsync(id, tokenSource.Token).ConfigureAwait(false));
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nThe operation was canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere was a problem retrieving data");
                Console.WriteLine($"\nERROR\n{ex.GetType()}\n{ex.Message}");
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }

        private static void HandleExit()
        {
            while (true)
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.X:
                        tokenSource.Cancel();
                        break;
                    case ConsoleKey.Q:
                        Console.WriteLine();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Waiting...");
                        break;
                }
        }
    }

}
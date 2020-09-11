using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace TaskException.UI
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
                Console.WriteLine("1) Use Task (parallel)      - await Exception");
                Console.WriteLine("2) Use Task (parallel)      - view AggregateException");
                Console.WriteLine("3) Use await (non-parallel) - await Exception");
                Console.WriteLine("(any other key to exit)");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine();
                        Task.Run(() => UseTaskAwaitException(tokenSource.Token));
                        HandleExit();
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine();
                        Task.Run(() => UseTaskAggregateException(tokenSource.Token));
                        HandleExit();
                        break;
                    case ConsoleKey.D3:
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

        private static async Task UseTaskAwaitException(CancellationToken cancelToken)
        {
            Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");

            var reader = new PersonReader();
            try
            {
                List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

                var taskList = new List<Task>();

                // Note: This loop generates 2 exceptions
                foreach (int id in ids)
                {
                    Task<Person> personTask = reader.GetPersonAsyncWithFailures(id, tokenSource.Token);
                    taskList.Add(personTask);

                    Task continuationTask = personTask.ContinueWith(task =>
                    {
                        Person person = task.Result;
                        Console.WriteLine(person.ToString());
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    taskList.Add(continuationTask);
                }

                await Task.WhenAll(taskList);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nThe operation was canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n(Error message from 'await')");
                OutputException(ex);
                Environment.Exit(1);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static async Task UseTaskAggregateException(CancellationToken cancelToken)
        {
            Console.WriteLine("One Moment Please ('x' to Cancel, 'q' to Quit)");

            var reader = new PersonReader();
            try
            {
                List<int> ids = await reader.GetIdsAsync().ConfigureAwait(false);

                var taskList = new List<Task>();

                // Note: This loop generates 2 exceptions
                foreach (int id in ids)
                {
                    Task<Person> personTask = reader.GetPersonAsyncWithFailures(id, tokenSource.Token);
                    taskList.Add(personTask);

                    Task continuationTask = personTask.ContinueWith(task =>
                    {
                        Person person = task.Result;
                        Console.WriteLine(person.ToString());
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                    taskList.Add(continuationTask);
                }

                await Task.WhenAll(taskList)
                    .ContinueWith(task =>
                    {
                        Console.WriteLine("\n(Error message from Task continuation)");
                        OutputException(task.Exception);
                    }, TaskContinuationOptions.OnlyOnFaulted)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nThe operation was canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n(Error message from 'await')");
                OutputException(ex);
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

                // Note: This loop generates 2 exceptions
                // However, "await" short-circuits the loop
                // at the first exception
                foreach (int id in ids)
                {
                    Person Person = await reader.GetPersonAsyncWithFailures(id, tokenSource.Token).ConfigureAwait(false);
                    Console.WriteLine(Person);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nThe operation was canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n(Error message from 'await')");
                OutputException(ex);
                Environment.Exit(1);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static void OutputException(Exception exception)
        {
            Console.WriteLine($"---");
            Console.WriteLine($"Exception Type:");
            Console.WriteLine($"    {exception.GetType()}");
            Console.WriteLine($"Exception Message:");
            Console.WriteLine($"    {exception.Message}");
            Console.WriteLine($"Inner Exception:");
            if (exception.InnerException == null)
            {
                Console.WriteLine($"    NONE");
            }
            else
            {
                Console.WriteLine($"    {exception.InnerException.GetType()}");
                Console.WriteLine($"    {exception.InnerException.Message}");
            }

            var aggEx = exception as AggregateException;
            if (aggEx != null)
            {
                Console.WriteLine($"Inner Exceptions Count: {aggEx.InnerExceptions.Count}");
                int current = 0;
                foreach (var ex in aggEx.Flatten().InnerExceptions)
                {
                    Console.WriteLine($"\nInnerExceptions[{current++}]:");
                    Console.WriteLine($"    {ex.GetType()}");
                    Console.WriteLine($"    {ex.Message}");
                }
            }

            Console.WriteLine($"---");
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
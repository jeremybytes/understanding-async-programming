using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Parallel.UI
{
    public partial class MainWindow : Window
    {
        PersonReader reader = new PersonReader();
        CancellationTokenSource? tokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
        {
            tokenSource = new CancellationTokenSource();
            FetchWithTaskButton.IsEnabled = false;
            ClearListBox();

            var taskList = new List<Task>();

            try
            {
                var ids = await reader.GetIdsAsync();

                foreach (int id in ids)
                {
                    Task<Person> personTask = reader.GetPersonAsync(id, tokenSource.Token);
                    taskList.Add(personTask);
                    _ = personTask.ContinueWith(task =>
                    {
                        Person person = task.Result;
                        PersonListBox.Items.Add(person);
                    },
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.FromCurrentSynchronizationContext());
                }

                await Task.WhenAll(taskList);
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"CANCELED\n{ex.GetType()}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR\n{ex.GetType()}\n{ex.Message}");
            }
            finally
            {
                tokenSource.Dispose();
                FetchWithTaskButton.IsEnabled = true;
            }
        }

        private async void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
        {
            tokenSource = new CancellationTokenSource();
            FetchWithAwaitButton.IsEnabled = false;
            try
            {
                ClearListBox();

                List<int> ids = await reader.GetIdsAsync();

                foreach (int id in ids)
                {
                    Person person = await reader.GetPersonAsync(id, tokenSource.Token);
                    PersonListBox.Items.Add(person);
                }
            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show($"CANCELED\n{ex.GetType()}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR\n{ex.GetType()}\n{ex.Message}");
            }
            finally
            {
                FetchWithAwaitButton.IsEnabled = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tokenSource?.Cancel();
            }
            catch (Exception)
            {
                // if there's something wrong with the 
                // cancellation token source, just ignore it
                // (it may be disposed because nothing is running)
            }
        }

        private void ClearListBox()
        {
            PersonListBox.Items.Clear();
        }
    }
}

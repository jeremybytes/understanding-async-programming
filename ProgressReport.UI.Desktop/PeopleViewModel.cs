using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ProgressReport.UI
{
    public class PeopleViewModel : INotifyPropertyChanged
    {
        PersonReader reader = new PersonReader();
        CancellationTokenSource? tokenSource;

        private List<Person> _people = new List<Person>();
        public List<Person> People
        {
            get { return _people; }
            set
            {
                if (_people == value)
                    return;
                _people = value;
                RaisePropertyChanged("People");
            }
        }

        private int _progressPercentage;
        public int ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                if (_progressPercentage == value)
                    return;
                _progressPercentage = value;
                RaisePropertyChanged("ProgressPercentage");
            }
        }

        private bool _fetchWithTaskEnabled = true;
        public bool FetchWithTaskEnabled
        {
            get { return _fetchWithTaskEnabled; }
            set
            {
                if (_fetchWithTaskEnabled == value)
                    return;
                _fetchWithTaskEnabled = value;
                RaisePropertyChanged("FetchWithTaskEnabled");
            }
        }

        private bool _fetchWithAwaitEnabled = true;
        public bool FetchWithAwaitEnabled
        {
            get { return _fetchWithAwaitEnabled; }
            set
            {
                if (_fetchWithAwaitEnabled == value)
                    return;
                _fetchWithAwaitEnabled = value;
                RaisePropertyChanged("FetchWithAwaitEnabled");
            }
        }

        public event EventHandler<NewUserMessageEventArgs>? NewUserMessage;
        protected virtual void OnNewUserMessage(NewUserMessageEventArgs e)
        {
            NewUserMessage?.Invoke(this, e);
        }

        public void FetchPeopleWithTask()
        {
            tokenSource = new CancellationTokenSource();
            FetchWithTaskEnabled = false;
            ClearListBox();

            var progress = new Progress<int>();
            progress.ProgressChanged += (_, e) => ProgressPercentage = e;

            Task<List<Person>> peopleTask =
                reader.GetPeopleAsync(progress, tokenSource.Token);

            peopleTask.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    foreach (var ex in task.Exception!.Flatten().InnerExceptions)
                    {
                        OnNewUserMessage(new NewUserMessageEventArgs("ERROR",
                            $"{ex.GetType()}\n{ex.Message}"));
                    }
                }
                if (task.IsCanceled)
                {
                    OnNewUserMessage(new NewUserMessageEventArgs("CANCELED", ""));
                }
                if (task.IsCompletedSuccessfully)
                {
                    People = task.Result;
                }

                tokenSource.Dispose();
                FetchWithTaskEnabled = true;
            },
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        public async Task FetchPeopleWithAwait()
        {
            using (tokenSource = new CancellationTokenSource())
            {
                FetchWithAwaitEnabled = false;
                try
                {
                    ClearListBox();

                    var progress = new Progress<int>();
                    progress.ProgressChanged += (_, e) => ProgressPercentage = e;

                    People =
                        await reader.GetPeopleAsync(progress, tokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                    OnNewUserMessage(new NewUserMessageEventArgs("CANCELED",
                        $"{ex.GetType()}\n{ex.Message}"));
                }
                catch (Exception ex)
                {
                    OnNewUserMessage(new NewUserMessageEventArgs("ERROR",
                        $"{ex.GetType()}\n{ex.Message}"));
                }
                finally
                {
                    FetchWithAwaitEnabled = true;
                }
            }
        }

        public void Cancel()
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
            People = new List<Person>();
            ProgressPercentage = 0;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class NewUserMessageEventArgs : EventArgs
    {
        public string Title { get; }
        public string Message { get; }

        public NewUserMessageEventArgs(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}

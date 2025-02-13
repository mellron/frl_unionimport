using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace frl_unionimport.util
{
    public class ConsoleQ : IDisposable
    {
        private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _processingTask;
        private bool _isProcessing;

        public ConsoleQ()
        {
            _isProcessing = true;
            _processingTask = Task.Run(ProcessQueueAsync);
        }

        public void WriteLine(string message)
        {
            _messageQueue.Enqueue(message);
        }

        public void WriteLine()
        {
            _messageQueue.Enqueue("\n");
        }

        private async Task ProcessQueueAsync()
        {
            while (_isProcessing)
            {
                if (_messageQueue.TryDequeue(out string message))
                {
                    Console.WriteLine(message);
                }
                else
                {
                    await Task.Delay(100); // Adjust delay as needed
                }
            }
        }

        public void StopProcessing()
        {
            _isProcessing = false;
            _cancellationTokenSource.Cancel();
            _processingTask.Wait();
        }

        public async Task WaitForCompletionAsync()
        {
            while (!_messageQueue.IsEmpty)
            {
                await Task.Delay(800); // Adjust delay as needed
            }
        }


        // Destructor (Finalizer)
        ~ConsoleQ()
        {
            StopProcessing();
        }

        // Implement IDisposable to allow for manual disposal
        public void Dispose()
        {
            StopProcessing();
            GC.SuppressFinalize(this);
        }
    }
}
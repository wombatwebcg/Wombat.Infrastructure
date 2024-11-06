using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wombat.Infrastructure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TimerWrapperHelper
    {
        private Timer _timer;
        private readonly TimeSpan _interval;
        private readonly Action _callback;
        private CancellationTokenSource _cancellationTokenSource;

        public TimerWrapperHelper(TimeSpan interval, Action callback)
        {
            _interval = interval;
            _callback = callback;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            return Task.Run(() =>
            {
                _timer = new Timer(Execute, null, TimeSpan.Zero, _interval);
                _cancellationTokenSource.Token.WaitHandle.WaitOne();
            });
        }

        private void Execute(object state)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _timer.Dispose();
                return;
            }

            _callback();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}

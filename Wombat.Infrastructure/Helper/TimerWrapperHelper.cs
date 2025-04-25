using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wombat.Infrastructure
{
    public class TimerWrapperHelper
    {
        private Timer _timer;
        private readonly TimeSpan _interval;
        private readonly Func<Task> _callback;
        private CancellationTokenSource _cancellationTokenSource;

        public TimerWrapperHelper(TimeSpan interval, Func<Task> callback)
        {
            _interval = interval;
            _callback = callback;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _timer = new Timer(Execute, null, TimeSpan.Zero, _interval);

            // 等待直到取消
            await Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token).ConfigureAwait(false);
        }

        private async void Execute(object state)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _timer.Dispose();
                return;
            }

            // 等待异步回调完成
            await _callback();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
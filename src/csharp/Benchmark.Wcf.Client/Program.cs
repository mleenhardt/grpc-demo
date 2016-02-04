using System;
using System.Threading;
using Benchmark.Wcf.Common;
using Timer = System.Timers.Timer;

namespace Benchmark.Wcf.Client
{
    class Program
    {
        private const int CALL_COUNT = 1000000;
        private static Timer _timer = new Timer(1000);
        private static int _lastMinuteCallCount;

        static void Main(string[] args)
        {
            using (var client = new BenchmarkServiceClient())
            {
                _timer.Elapsed += (s, e) =>
                {
                    var lastMinuteCallCount = Interlocked.Exchange(ref _lastMinuteCallCount, 0);
                    Console.WriteLine($"{lastMinuteCallCount} ops/sec");
                };
                _timer.Start();

                for (int i = 0; i < CALL_COUNT; i++)
                {
                    client.Proxy.Operation(new ServiceRequest { Id = 10 });
                    Interlocked.Increment(ref _lastMinuteCallCount);
                }
            }
        }
    }
}

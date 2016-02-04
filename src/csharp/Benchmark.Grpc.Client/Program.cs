using System;
using System.Threading;
using Benchmark.Grpc.Common;
using Grpc.Core;
using Timer = System.Timers.Timer;

namespace Benchmark.Grpc.Client
{
    class Program
    {
        private const int CALL_COUNT = 1000000;
        private static Timer _timer = new Timer(1000);
        private static int _lastMinuteCallCount;

        static void Main(string[] args)
        {
            var rpcChannel = new Channel("localhost:1337", ChannelCredentials.Insecure);
            var rpcClient = new BenchmarkServiceClient(BenchmarkService.NewClient(rpcChannel));
            rpcChannel.ConnectAsync().Wait();

            _timer.Elapsed += (s, e) =>
            {
                var lastMinuteCallCount = Interlocked.Exchange(ref _lastMinuteCallCount, 0);
                Console.WriteLine($"{lastMinuteCallCount} ops/sec");
            };
            _timer.Start();

            for (int i = 0; i < CALL_COUNT; i++)
            {
                rpcClient.Operation(new ServiceRequest { Id = 10 });
                Interlocked.Increment(ref _lastMinuteCallCount);
            }

            //for (int i = 0; i < CALL_COUNT; i++)
            //{
            //    rpcClient.OperationAsync(new ServiceRequest { Id = 10 }).ContinueWith(t => Interlocked.Increment(ref _lastMinuteCallCount));
            //}

            //rpcClient.OperationStreamAsync(() => Interlocked.Increment(ref _lastMinuteCallCount)).Wait();
        }
    }
}

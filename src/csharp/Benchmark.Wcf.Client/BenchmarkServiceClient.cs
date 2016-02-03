using System.ServiceModel;
using Benchmark.Wcf.Common;

namespace Benchmark.Wcf.Client
{
    public class BenchmarkServiceClient : ClientBase<IBenchmarkService>
    {
        public IBenchmarkService Proxy => Channel;
    }
}
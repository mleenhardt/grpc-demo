using Benchmark.Wcf.Common;

namespace Benchmark.Wcf.Server
{
    public class BenchmarkService : IBenchmarkService
    {
        public ServiceResponse Operation(ServiceRequest request)
        {
            return new ServiceResponse { Id = request.Id };
        }
    }
}
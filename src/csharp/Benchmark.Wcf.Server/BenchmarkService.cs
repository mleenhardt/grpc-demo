using System.Threading.Tasks;
using Benchmark.Wcf.Common;

namespace Benchmark.Wcf.Server
{
    public class BenchmarkService : IBenchmarkService
    {
        public ServiceResponse Operation(ServiceRequest request)
        {
            return new ServiceResponse { Id = request.Id };
        }

        public Task<ServiceResponse> OperationTaskAsync(ServiceRequest request)
        {
            return Task.FromResult(new ServiceResponse { Id = request.Id });
        }
    }
}
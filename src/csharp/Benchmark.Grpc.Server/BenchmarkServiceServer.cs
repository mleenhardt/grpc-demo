using System.Collections.Generic;
using System.Threading.Tasks;
using Benchmark.Grpc.Common;
using Grpc.Core;

namespace Benchmark.Grpc.Server
{
    public class BenchmarkServiceServer : BenchmarkService.IBenchmarkService
    {
        public Task<ServiceResponse> Operation(ServiceRequest request, ServerCallContext context)
        {
            return Task.FromResult(new ServiceResponse { Id = request.Id });
        }

        public async Task OperationStream(IAsyncStreamReader<ServiceRequest> requestStream, IServerStreamWriter<ServiceResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                await responseStream.WriteAsync(new ServiceResponse { Id = requestStream.Current.Id });
            }
        }
    }
}
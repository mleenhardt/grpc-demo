using System;
using System.Threading;
using System.Threading.Tasks;
using Benchmark.Grpc.Common;

namespace Benchmark.Grpc.Client
{
    public class BenchmarkServiceClient
    {
        private readonly BenchmarkService.IBenchmarkServiceClient _grpcClient;

        public BenchmarkServiceClient(BenchmarkService.IBenchmarkServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public ServiceResponse Operation(ServiceRequest serviceRequest)
        {
            return _grpcClient.Operation(serviceRequest);
        }

        public Task<ServiceResponse> OperationAsync(ServiceRequest serviceRequest)
        {
            return _grpcClient.OperationAsync(serviceRequest).ResponseAsync;
        }

        public async Task OperationStreamAsync(Action afterRoundTrip)
        {
            using (var call = _grpcClient.OperationStream())
            {
                await call.RequestStream.WriteAsync(new ServiceRequest { Id = 10 });
                while (await call.ResponseStream.MoveNext(CancellationToken.None))
                {
                    afterRoundTrip();
                    await call.RequestStream.WriteAsync(new ServiceRequest { Id = call.ResponseStream.Current.Id });
                }
            }
        }
    }
}
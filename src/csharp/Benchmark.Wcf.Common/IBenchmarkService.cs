using System.ServiceModel;

namespace Benchmark.Wcf.Common
{
    [ServiceContract]
    public interface IBenchmarkService
    {
        [OperationContract]
        ServiceResponse Operation(ServiceRequest request);
    }
}
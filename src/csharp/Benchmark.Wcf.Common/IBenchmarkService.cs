using System.ServiceModel;
using System.Threading.Tasks;

namespace Benchmark.Wcf.Common
{
    [ServiceContract]
    public interface IBenchmarkService
    {
        [OperationContract]
        ServiceResponse Operation(ServiceRequest request);
        [OperationContract]
        Task<ServiceResponse> OperationTaskAsync(ServiceRequest request);
    }
}
using System.Runtime.Serialization;

namespace Benchmark.Wcf.Common
{
    [DataContract]
    public class ServiceRequest
    {
        [DataMember]
        public int Id { get; set; } 
    }
}
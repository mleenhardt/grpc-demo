using System.Runtime.Serialization;

namespace Benchmark.Wcf.Common
{
    [DataContract]
    public class ServiceResponse
    {
        [DataMember]
        public int Id { get; set; } 
    }
}
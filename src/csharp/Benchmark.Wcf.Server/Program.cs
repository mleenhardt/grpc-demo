using System;
using System.ServiceModel;

namespace Benchmark.Wcf.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(BenchmarkService));
            serviceHost.Open();
            Console.WriteLine("service host open");
            Console.ReadLine();
            serviceHost.Close();
            Console.WriteLine("exiting");
        }
    }
}

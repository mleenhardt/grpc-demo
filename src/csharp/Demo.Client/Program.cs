using System;
using Demo.Common.ServiceDefinition;
using Grpc.Core;

namespace Demo.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var rpcChannel = new Channel("localhost:1337", ChannelCredentials.Insecure);
            var rpcClient = new GameAdminServiceClient(GameAdminService.NewClient(rpcChannel));

            rpcChannel.ConnectAsync().Wait();
            Log($"GameAdminServiceClient connected to {rpcChannel.ResolvedTarget}, channel state = {rpcChannel.State}");


            Log($"GameAdminServiceClient disconnecting from {rpcChannel.ResolvedTarget}, channel state = {rpcChannel.State}");
            rpcChannel.ShutdownAsync().Wait();

            Console.WriteLine("Press any key to stop the client...");
            Console.ReadKey();
        }

        static void Log(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow} -- {message}");
        }
    }
}

using System;
using Demo.Common.ServiceDefinition;
using Grpc.Core;

namespace Demo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameAdminService = new GameAdminServiceServer(
                new HardcodedAccountRepository(),
                new HardcodedChatMessageRepository());

            const int port = 1337;
            var rpcServer = new Grpc.Core.Server
            {
                Services = { GameAdminService.BindService(gameAdminService) },
                Ports = { new Grpc.Core.ServerPort("localhost", port, ServerCredentials.Insecure) }
            };

            Console.WriteLine("RouteGuide server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            rpcServer.ShutdownAsync().Wait();
        }
    }
}

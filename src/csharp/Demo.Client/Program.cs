using System;
using System.Linq;
using System.Threading;
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

            rpcClient.GetAccountAsync(1234).Wait();
            rpcClient.GetChatHistoryAsync(Enumerable.Range(1, 2), CancellationToken.None).Wait();
            rpcClient.ListenChatAsync(1234).Wait();
            rpcClient.ChatAsync().Wait();

            Log($"GameAdminServiceClient disconnecting from {rpcChannel.ResolvedTarget}, channel state = {rpcChannel.State}", true);
            rpcChannel.ShutdownAsync().Wait();

            Console.WriteLine("Press any key to stop the client...");
            Console.ReadKey();
        }

        public static void Log(string message, bool addLineBreak = false)
        {
            if (addLineBreak)
            {
                Console.WriteLine(Environment.NewLine);
            }
            Console.WriteLine($"{DateTime.UtcNow} -- {message}");
        }
    }
}

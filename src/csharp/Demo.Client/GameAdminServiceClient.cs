using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Demo.Common;
using Demo.Common.ServiceDefinition;
using Grpc.Core;

namespace Demo.Client
{
    public sealed class GameAdminServiceClient
    {
        private readonly GameAdminService.IGameAdminServiceClient _grpcClient;

        public GameAdminServiceClient(GameAdminService.IGameAdminServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            Program.Log($"Starting RPC GetAccountAsync (accountId {accountId})", true);
            AsyncUnaryCall<Account> call = _grpcClient.GetAccountAsync(new AccountRequest { AccountId = accountId });
            Account account = await call.ResponseAsync;
            Program.Log($"RPC GetAccountAsync received {account}");
            return account;
        }

        public async Task<ICollection<ChatMessage>> GetChatHistoryAsync(IEnumerable<int> accountIds, CancellationToken cancellationToken)
        {
            Program.Log($"Starting RPC GetChatHistoryAsync (accountIds {String.Join(",", accountIds)})", true);
            using (var call = _grpcClient.GetChatHistory(deadline: DateTime.UtcNow.AddMinutes(2), cancellationToken: cancellationToken))
            {
                foreach (int accountid in accountIds)
                {
                    await call.RequestStream.WriteAsync(new ChatMessageRequest { AccountId = accountid });
                    // call.RequestStream.WriteAllAsync() and other async stream extensions are available.
                }

                // Completing call, which completes the async enumerator that the server
                // is enumerating and allows it to send its response. 
                await call.RequestStream.CompleteAsync();

                ChatMessageCollection response = await call.ResponseAsync;
                Program.Log($"RPC GetChatHistoryAsync received {response}");
                return response.ChatMessages;
            }
        }

        public async Task<ICollection<ChatMessage>> ListenChatAsync(int accountId)
        {
            Program.Log($"Starting RPC ListenChatAsync (accountId {accountId})", true);

            // parameters can be passed to call one by one or be composed 
            // into a CallOptions using a fluent syntax.
            var callOptions = new CallOptions()
                .WithCancellationToken(CancellationToken.None)
                .WithDeadline(DateTime.UtcNow.AddMinutes(2))
                .WithHeaders(Metadata.Empty);

            using (var call = _grpcClient.ListenChat(new ChatMessageRequest { AccountId = accountId }, callOptions))
            {
                // Custom response header
                Metadata responseHeader = await call.ResponseHeadersAsync;
                Program.Log($"ListenChatAsync response header {String.Join(",", responseHeader.Select(m => m.ToString()))}");

                var chatMessages = new List<ChatMessage>();
                while (await call.ResponseStream.MoveNext())
                {
                    ChatMessage chatMessage = call.ResponseStream.Current;
                    Program.Log($"RPC ListenChatAsync received {chatMessage}");
                    chatMessages.Add(chatMessage);
                }

                // Custom response trailer
                Metadata responseTrailer = call.GetTrailers();
                Program.Log($"ListenChatAsync response trailer {String.Join(",", responseTrailer.Select(m => m.ToString()))}");

                return chatMessages;
            }
        }

        public async Task ChatAsync()
        {
            Program.Log("Starting RPC ChatAsync", true);

            const int maxChatCount = 5;

            using (var call = _grpcClient.Chat())
            {
                var responseReaderTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        ChatMessage serverChatMessage = call.ResponseStream.Current;
                        Program.Log($"Server says {serverChatMessage}");
                    }
                });

                for (int i = 0; i < maxChatCount; i++)
                {
                    await call.RequestStream.WriteAsync(Utility.GetRandomChatMessage(0));
                }

                await call.RequestStream.CompleteAsync();
                await responseReaderTask;
            }
        }
    }
}
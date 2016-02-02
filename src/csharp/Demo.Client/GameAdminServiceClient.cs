using System;
using System.Collections.Generic;
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

        public Task<Account> GetAccountAsync(int accountId)
        {
            AsyncUnaryCall<Account> call = _grpcClient.GetAccountAsync(new AccountRequest { AccountId = accountId });
            return call.ResponseAsync;
        }

        public async Task<ICollection<ChatMessage>> GetChatHistoryAsync(IEnumerable<int> accountIds, CancellationToken cancellationToken)
        {
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
                return response.ChatMessages;
            }
        }

        public async Task<ICollection<ChatMessage>> ListenChatAsync(int accountId)
        {
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

                var chatMessages = new List<ChatMessage>();
                while (await call.ResponseStream.MoveNext())
                {
                    ChatMessage chatMessage = call.ResponseStream.Current;
                    chatMessages.Add(chatMessage);
                }

                // Custom response trailer
                Metadata responseTrailer = call.GetTrailers();

                return chatMessages;
            }
        }

        public async Task ChatAsync()
        {
            const int maxChatCount = 10;

            using (var call = _grpcClient.Chat())
            {
                var responseReaderTask = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        ChatMessage serverChatMessage = call.ResponseStream.Current;
                        // log
                    }
                });

                for (int i = 0; i < maxChatCount; i++)
                {
                    await call.RequestStream.WriteAsync(Utility.GetRandomChatMessage(0));
                    // log
                }

                await call.RequestStream.CompleteAsync();
                await responseReaderTask;
            }
        }
    }
}
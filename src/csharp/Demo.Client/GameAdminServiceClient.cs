using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
                }
                
                // Completing call, which completes the async enumerator that the server
                // is enumerating and allows it to send its response. 
                await call.RequestStream.CompleteAsync();

                ChatMessageCollection response = await call.ResponseAsync;
                return response.ChatMessages;
            }
        }

        public async Task SomeMethod(CancellationToken cancellationToken)
        {
            
        }
    }
}
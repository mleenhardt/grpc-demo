using System;
using System.Threading.Tasks;
using Demo.Common.ServiceDefinition;
using Grpc.Core;

namespace Demo.Server
{
    public sealed class GameAdminServiceServer : GameAdminService.IGameAdminService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IChatMessageRepository _chatMessageRepository;
         
        public GameAdminServiceServer(IAccountRepository accountRepository, IChatMessageRepository chatMessageRepository)
        {
            _accountRepository = accountRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        private static void Log(ServerCallContext context)
        {
            Console.WriteLine($"{DateTime.UtcNow} -- RPC call, method={context.Method}, host={context.Host}, " +
                              $"peer={context.Peer}, headers={context.RequestHeaders}");
        }

        public Task<Account> GetAccount(AccountRequest request, ServerCallContext context)
        {
            Log(context);
            return _accountRepository.GetByIdAsync(request.AccountId);
        }

        public Task<ChatMessageCollection> GetChatHistory(IAsyncStreamReader<ChatMessageRequest> requestStream, ServerCallContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task ListenChat(ChatMessageRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
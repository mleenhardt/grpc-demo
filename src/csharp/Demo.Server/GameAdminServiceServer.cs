using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
                              $"peer={context.Peer}, headers={String.Join(", ", context.RequestHeaders.Select(h => h.ToString()))}");
        }

        public Task<Account> GetAccount(AccountRequest request, ServerCallContext context)
        {
            Log(context);
            return _accountRepository.GetByIdAsync(request.AccountId);
        }

        public async Task<ChatMessageCollection> GetChatHistory(IAsyncStreamReader<ChatMessageRequest> requestStream, ServerCallContext context)
        {
            Log(context);
            var responses = new List<ChatMessage>();
            // Async enumerator
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                ChatMessageRequest chatMessageRequest = requestStream.Current;
                ICollection<ChatMessage> chatMessages = await _chatMessageRepository.GetAccountChatHistoryAsync(chatMessageRequest.AccountId);
                responses.AddRange(chatMessages);
            }
            return new ChatMessageCollection { ChatMessages = { responses } };
        }

        public async Task ListenChat(ChatMessageRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            Log(context);
            using (IAsyncEnumerator<ChatMessage> enumerator = _chatMessageRepository.ListenAccountChatAsync(request.AccountId).GetEnumerator())
            {
                // Async enumerator
                while (await enumerator.MoveNext())
                {
                    ChatMessage chatMessage = enumerator.Current;
                    await responseStream.WriteAsync(chatMessage);
                }

                // Custom response trailer
                context.ResponseTrailers.Add(new Metadata.Entry("Some response trailer key", "Some response trailer value"));
            }
        }

        public Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
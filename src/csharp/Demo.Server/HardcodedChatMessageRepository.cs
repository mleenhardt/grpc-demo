using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using System.Threading.Tasks;
using System.Timers;
using Demo.Common.ServiceDefinition;

namespace Demo.Server
{
    public sealed class HardcodedChatMessageRepository : IChatMessageRepository
    {
        private static ChatMessage GetRandomChatMessage(int accountId)
        {
            int characterId = new Random().Next();
            return new ChatMessage
            {
                CharacterId = characterId,
                Message = $"This is a random test message from account {accountId}, characterId {characterId}",
                TimestampUtc = DateTime.UtcNow.Ticks
            };
        }

        public Task<ICollection<ChatMessage>> GetAccountChatHistoryAsync(int accountId)
        {
            var chatMessages = Enumerable.Range(0, 10).Select(_ => GetRandomChatMessage(accountId)).ToList() as ICollection<ChatMessage>;
            return Task.FromResult(chatMessages);
        }

        public IAsyncEnumerable<ChatMessage> ListenAccountChatAsync(int accountId)
        {
            var observable = Observable.Create<ChatMessage>(async observer =>
            {
                for (int i = 0; i < 10; i++)
                {
                    observer.OnNext(GetRandomChatMessage(accountId));
                    await Task.Delay(500);
                }
                observer.OnCompleted();
            });
            return observable.ToAsyncEnumerable();
        }
    }
}
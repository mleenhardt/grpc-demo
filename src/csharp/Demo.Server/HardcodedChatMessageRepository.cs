using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Demo.Common;
using Demo.Common.ServiceDefinition;

namespace Demo.Server
{
    public sealed class HardcodedChatMessageRepository : IChatMessageRepository
    {
        public Task<ICollection<ChatMessage>> GetAccountChatHistoryAsync(int accountId)
        {
            var chatMessages = Enumerable.Range(0, 3).Select(_ => Utility.GetRandomChatMessage(accountId)).ToList() as ICollection<ChatMessage>;
            return Task.FromResult(chatMessages);
        }

        public IAsyncEnumerable<ChatMessage> ListenAccountChatAsync(int accountId)
        {
            var observable = Observable.Create<ChatMessage>(async observer =>
            {
                for (int i = 0; i < 5; i++)
                {
                    observer.OnNext(Utility.GetRandomChatMessage(accountId));
                    await Task.Delay(500);
                }
                observer.OnCompleted();
            });
            return observable.ToAsyncEnumerable();
        }
    }
}
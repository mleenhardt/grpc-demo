using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Common.ServiceDefinition;

namespace Demo.Server
{
    public interface IChatMessageRepository
    {
        Task<ICollection<ChatMessage>> GetAccountChatHistoryAsync(int accountId);
        IAsyncEnumerable<ChatMessage> ListenAccountChatAsync(int accountId);
    }
}
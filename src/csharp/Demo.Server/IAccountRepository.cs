using System.Threading.Tasks;
using Demo.Common.ServiceDefinition;

namespace Demo.Server
{
    public interface IAccountRepository
    {
        Task<Account> GetByIdAsync(int accountId);
    }
}
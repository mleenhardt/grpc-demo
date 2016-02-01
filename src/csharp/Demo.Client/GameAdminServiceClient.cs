using System.Threading.Tasks;
using Demo.Common.ServiceDefinition;

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
            return _grpcClient.GetAccountAsync(new AccountRequest { AccountId = accountId }).ResponseAsync;
        } 
    }
}
using System;
using System.Threading.Tasks;
using Demo.Common;
using Demo.Common.ServiceDefinition;

namespace Demo.Server
{
    public sealed class HardcodedAccountRepository : IAccountRepository
    {
        public Task<Account> GetByIdAsync(int accountId)
        {
            var random = new Random();
            var account = new Account
            {
                Id = accountId,
                Email = String.Concat(Utility.GetRandomString(random.Next(2, 8)), "@", Utility.GetRandomString(random.Next(2, 8)), ".com"),
            };
            for (int i = 0; i < random.Next(1, 5); i++)
            {
                account.Characters.Add(new Character
                {
                    Id = random.Next(),
                    Name = Utility.GetRandomString(random.Next(3, 10))
                });
            }
            return Task.FromResult(account);
        }
    }
}
using System;
using System.Linq;
using Demo.Common.ServiceDefinition;

namespace Demo.Common
{
    public static class Utility
    {
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static ChatMessage GetRandomChatMessage(int accountId)
        {
            int characterId = new Random().Next();
            return new ChatMessage
            {
                CharacterId = characterId,
                Message = $"This is a random test message from account {accountId}, characterId {characterId}",
                TimestampUtc = DateTime.UtcNow.Ticks
            };
        }
    }
}
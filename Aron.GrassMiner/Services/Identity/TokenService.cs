using Microsoft.Extensions.Caching.Memory;

namespace Aron.GrassMiner.Services.Identity
{
    public class TokenService
    {
        private readonly IMemoryCache _cache;

        public TokenService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void StoreToken(string token, TimeSpan tokenLifetime)
        {
            _cache.Set(token, true, tokenLifetime);
        }

        public bool IsTokenValid(string token)
        {
            return _cache.TryGetValue(token, out _);
        }

        public void RemoveToken(string token)
        {
            _cache.Remove(token);
        }
    }
}

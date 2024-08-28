using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace BookCatalog.Core.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheResourceFilterAttribute : Attribute, IResourceFilter //There is another interface as async IAsyncResourceFilter \
                                                                           //in this interface a method is enough to implement
    {
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey;

        public CacheResourceFilterAttribute( string cacheKey)
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _cacheKey = cacheKey;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if(_cache.TryGetValue(_cacheKey, out var cachedResult))
            {
                context.Result = cachedResult as IActionResult;
            }
        }
      

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if(context.Result is IActionResult result)
            {
                var option = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                _cache.Set(_cacheKey, result, option);
            }
        }
    }
}

using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace WalletApp.Infrastructure.Services.MemoryCach
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(string pattern, ICacheManager cacheManager)
        {
            _pattern = pattern;
            _cacheManager = cacheManager;
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}

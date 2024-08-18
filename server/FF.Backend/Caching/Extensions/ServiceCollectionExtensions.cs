using Microsoft.Extensions.DependencyInjection;

namespace FF.Backend.Caching.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCaching(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
        }

    }
}

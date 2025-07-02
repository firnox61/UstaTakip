using System.Diagnostics;
using UstaTakip.Core.Caching;
using UstaTakip.Core.Utilities.IoC;

namespace UstaTakip.WebAPI.DependencyInjection
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache();//MemoryCacheManager deki Imemoryinterfacimiin karşılığı var

            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();//senden cahchemanager isterse momorycachemanager ver
            serviceCollection.AddSingleton<Stopwatch>();//performance iççin

        }
    }
}

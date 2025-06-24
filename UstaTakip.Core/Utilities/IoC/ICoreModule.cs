using Microsoft.Extensions.DependencyInjection;

namespace UstaTakip.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}

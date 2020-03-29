using Acr.UserDialogs;
using Chat.Clinet.Core.Interfaces;
using Chat.Clinet.Core.Services;
using MvvmCross.IoC;

namespace Chat.Clinet.Core
{
    public static class SetupIoC
    {
        public static IMvxIoCProvider RegisterDependencies(IMvxIoCProvider provider)
        {
            provider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            provider.ConstructAndRegisterSingleton<IClientService, ClientService>();
            return provider;
        }
    }
}

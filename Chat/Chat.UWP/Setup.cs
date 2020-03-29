using Chat.Clinet.Core;
using MvvmCross.Forms.Platforms.Uap.Core;
using MvvmCross.IoC;

namespace Chat.UWP
{
    public class Setup : MvxFormsWindowsSetup<Chat.Clinet.Core.App, Chat.App>
    {
        protected override IMvxIoCProvider InitializeIoC()
        {
            var provider = base.InitializeIoC();
            return SetupIoC.RegisterDependencies(provider);
        }
    }
}

using MvvmCross;
using MvvmCross.Plugin;

namespace MvvX.Plugins.IOAuthClient.Wpf
{
    [MvxPlugin]
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.IoCProvider.RegisterSingleton<IOAuthClient>(new PlatformOAuthClient());
        }
    }
}

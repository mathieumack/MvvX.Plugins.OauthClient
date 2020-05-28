using MvvmCross;
using MvvmCross.Plugin;

namespace MvvX.Plugins.OAuthClient
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
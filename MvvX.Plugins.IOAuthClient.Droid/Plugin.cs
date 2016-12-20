using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace MvvX.Plugins.IOAuthClient.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IOAuthClient>(new PlatformOAuthClient());
        }
    }
}
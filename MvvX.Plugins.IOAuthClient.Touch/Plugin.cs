using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvX.Plugins.IOAuthClient.Touch
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IOAuthClient>(new PlatformOAuthClient());
        }
    }
}

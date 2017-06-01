# MvvX.Plugins.OAuthClient
OAuth client for MvvMCross

## Build

[![Build status](https://ci.appveyor.com/api/projects/status/cl6w3d37sjc2123b?svg=true)](https://ci.appveyor.com/project/mathieumack/mvvx-plugins-ioauthclient)

## Nuget

[![NuGet package](https://buildstats.info/nuget/MvvX.Plugins.OAuthClient?includePreReleases=true)](https://nuget.org/packages/MvvX.Plugins.OAuthClient)

## How to use it ?
Create an instance of IOAuthClient with Mvx.Resolve<IOAuthClient>();

### WPF
Configure it with the New method :

```c#
var auth = Mvx.Resolve<IOAuthClient>();
auth.New(this,
    "temporaryKey",
    "<client_id>",
    "<client_secret>",
    "",
    new Uri("<authorization_uri>"),
    new Uri("<redirect_uri>"),
    new Uri("<token_uri>"));
```

You can configure the MaxWidth and MaxHeight of the popin by adding app settings in your app.config file :

```c#
  <appSettings>
    <add key="MvvX.Plugins.IOAuthClient.Wpf.Window.MaxWidth" value="800"/>
    <add key="MvvX.Plugins.IOAuthClient.Wpf.Window.MaxHeight" value="600"/>
  </appSettings>
```

MvvX.Plugins.IOAuthClient.Wpf.Window.MaxWidth key corresponds to the MaxWidth

MvvX.Plugins.IOAuthClient.Wpf.Window.MaxHeight key corresponds to the MaxHeight

### Android
To do

### iOS
To do
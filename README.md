# MvvX.Plugins.OAuthClient
OAuth client for MvvMCross as plugin.

# IC
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=github-MvvX.Plugins.OauthClient&metric=alert_status)](https://sonarcloud.io/dashboard?id=github-MvvX.Plugins.OauthClient)
[![Build status](https://dev.azure.com/mackmathieu/Github/_apis/build/status/MvvX.Plugins.OauthClient)](https://dev.azure.com/mackmathieu/Github/_build/latest?definitionId=18)
[![NuGet package](https://buildstats.info/nuget/MvvX.Plugins.OAuthClient?includePreReleases=true)](https://nuget.org/packages/MvvX.Plugins.OAuthClient)

## How to use it ?
Create an instance of IOAuthClient by using :
```c#
Mvx.IocProvider.Resolve<IOAuthClient>();
```

If you dont use MvvmCross, you can also create a new instance directly :
```c#
var auth = new PlatformOAuthClient();
```

It work with :
* WPF
* Android
* iOS

## WPF :

```c#
var auth = Mvx.IocProvider.Resolve<IOAuthClient>();
auth.New(this,
                "temporaryKey",
                clientId: "<ClientID>",
                scope: "<scope>",
                authorizeUrl: new Uri("<AuthorizeUrl>"),
                redirectUrl: new Uri("<RedirectUrl>"));
```

You can configure the MaxWidth and MaxHeight of the popin by adding app settings in your app.config file :

```c#
  <appSettings>
    <add key="MvvX.Plugins.OAuthClient.Wpf.Window.MaxWidth" value="800"/>
    <add key="MvvX.Plugins.OAuthClient.Wpf.Window.MaxHeight" value="600"/>
  </appSettings>
```

MvvX.Plugins.OAuthClient.Wpf.Window.MaxWidth key corresponds to the MaxWidth

MvvX.Plugins.OAuthClient.Wpf.Window.MaxHeight key corresponds to the MaxHeight

## Android

In an activity
```c#
var auth = Mvx.IocProvider.Resolve<IOAuthClient>();
auth.New(this,
            "temporaryKey",
            "<client_id>",
            "<client_secret>",
            "",
            new Uri("<authorization_uri>"),
            new Uri("<redirect_uri>"),
            new Uri("<token_uri>"));
```
"this" corresponds to the current activity.

You can found a sample call in the Droid sample app.

## iOS

```c#
var auth = Mvx.IocProvider.Resolve<IOAuthClient>();
auth.New(dialog,
            "temporaryKey",
            "<client_id>",
            scope: "",
            new Uri("<authorization_uri>"),
            new Uri("<redirect_uri>"));
```

dialog must corresponds to a DialogViewController

You can found a sample call in the Touch sample app.
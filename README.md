[![Build status](https://ci.appveyor.com/api/projects/status/cl6w3d37sjc2123b?svg=true)](https://ci.appveyor.com/project/mathieumack/mvvx-plugins-ioauthclient)

# MvvX.Plugins.OAuthClient
OAuth client for MvvMCross

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

### Android
To do

### iOS
To do
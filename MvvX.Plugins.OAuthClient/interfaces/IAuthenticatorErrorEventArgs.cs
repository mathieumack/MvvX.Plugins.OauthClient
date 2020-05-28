using System;

namespace MvvX.Plugins.OAuthClient
{
    public interface IAuthenticatorErrorEventArgs
    {
        Exception Exception { get; }

        string Message { get; }
    }
}

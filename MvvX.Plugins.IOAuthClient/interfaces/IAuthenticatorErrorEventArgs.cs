using System;

namespace MvvX.Plugins.IOAuthClient
{
    public interface IAuthenticatorErrorEventArgs
    {
        Exception Exception { get; }

        string Message { get; }
    }
}

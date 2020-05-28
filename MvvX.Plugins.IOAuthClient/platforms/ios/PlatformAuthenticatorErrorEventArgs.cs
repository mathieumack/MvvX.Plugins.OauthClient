using System;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient
{
    public class PlatformAuthenticatorErrorEventArgs : IAuthenticatorErrorEventArgs
    {
        #region Fields

        private readonly AuthenticatorErrorEventArgs eventArgs;
        
        public Exception Exception
        {
            get
            {
                return eventArgs.Exception;
            }
        }

        public string Message
        {
            get
            {
                return eventArgs.Message;
            }
        }

        #endregion

        #region Constructor

        public PlatformAuthenticatorErrorEventArgs(AuthenticatorErrorEventArgs eventArgs)
        {
            this.eventArgs = eventArgs;
        }

        #endregion
    }
}
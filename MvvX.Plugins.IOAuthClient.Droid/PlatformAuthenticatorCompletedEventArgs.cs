using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Droid
{
    public class PlatformAuthenticatorCompletedEventArgs : IAuthenticatorCompletedEventArgs
    {
        #region Fields

        private readonly AuthenticatorCompletedEventArgs eventArgs;

        private IAccount account;
        public IAccount Account
        {
            get
            {
                if (account == null)
                    account = new PlatformAccount(eventArgs.Account);
                return account;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return eventArgs.IsAuthenticated;
            }
        }

        #endregion

        #region Constructor

        public PlatformAuthenticatorCompletedEventArgs(AuthenticatorCompletedEventArgs eventArgs)
        {
            this.eventArgs = eventArgs;
        }

        #endregion
    }
}
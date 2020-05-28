using System.Collections.Generic;
using System.Net;
using Xamarin.Auth;

namespace MvvX.Plugins.OAuthClient
{
    public class PlatformAccount : IAccount
    {
        #region Fields

        private readonly Account account;

        public Dictionary<string, string> Properties
        {
            get
            {
                if (account == null)
                    return new Dictionary<string, string>();
                else
                    return account.Properties;
            }
        }

        public string Username
        {
            get
            {
                if (account == null)
                    return null;
                else
                    return account.Username;
            }

            set
            {
                account.Username = value;
            }
        }

        public CookieContainer Cookies
        {
            get
            {
                if (account == null)
                    return null;
                else
                    return account.Cookies;
            }
        }

        public string Serialize()
        {
            return account.Serialize();
        }

        #endregion

        #region Constructor

        public PlatformAccount(Account account)
        {
            this.account = account;
        }

        #endregion
    }
}
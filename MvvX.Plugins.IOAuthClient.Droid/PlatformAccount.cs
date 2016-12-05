using System;
using System.Collections.Generic;
using System.Net;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Droid
{
    public class PlatformAccount : IAccount
    {
        #region Fields

        private readonly Account account;
        
        public Dictionary<string, string> Properties
        {
            get
            {
                return account.Properties;
            }
        }

        public string Username
        {
            get
            {
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
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient
{
    public class PlatformOAuthClient : IOAuthClient
    {
        #region Fields

        private Account account;

        private CustomOAuth2Authenticator auth;
        private Context context;
        private string accountStoreKeyName;

        public bool AllowCancel
        {
            get
            {
                return auth.AllowCancel;
            }

            set
            {
                auth.AllowCancel = value;
            }
        }

        public string AccessTokenName
        {
            get
            {
                return auth.AccessTokenName;
            }

            set
            {
                auth.AccessTokenName = value;
            }
        }

        public string ClientId
        {
            get
            {
                return auth.ClientId;
            }
        }

        public string ClientSecret
        {
            get
            {
                return auth.ClientSecret;
            }
        }

        public bool DoNotEscapeScope
        {
            get
            {
                return auth.DoNotEscapeScope;
            }

            set
            {
                auth.DoNotEscapeScope = value;
            }
        }

        public Dictionary<string, string> RequestParameters
        {
            get
            {
                return auth.RequestParameters;
            }
        }

        #endregion

        #region Events

        public event EventHandler<IAuthenticatorCompletedEventArgs> Completed;

        private void OAuth2Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            account = e.Account;
            if (e.IsAuthenticated)
                AccountStore.Create(context).Save(e.Account, accountStoreKeyName);

            if (Completed != null)
            {
                Completed(sender, new PlatformAuthenticatorCompletedEventArgs(e));
            }
        }

        public event EventHandler<IAuthenticatorErrorEventArgs> Error;

        private void OAuth2Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            if (Error != null)
            {
                Error(sender, new PlatformAuthenticatorErrorEventArgs(e));
            }
        }

        #endregion

        #region Methods

        public void Start(string screenTitle)
        {
            var intent = auth.GetUI(context);
            context.StartActivity(intent);
        }

        public void New(object parameter, string accountStoreKeyName, string clientId, string scope, Uri authorizeUrl, Uri redirectUrl)
        {
            if (auth != null)
            {
                auth.Completed -= OAuth2Authenticator_Completed;
                auth.Error -= OAuth2Authenticator_Error;
            }

            this.accountStoreKeyName = accountStoreKeyName;

            if (!(parameter is Context))
                throw new ArgumentException("parameter must be a Context object");

            context = parameter as Context;

            LoadAccount();

            auth = new CustomOAuth2Authenticator(
                clientId: clientId,
                scope: scope,
                authorizeUrl: authorizeUrl,
                redirectUrl: redirectUrl);

            auth.Completed += OAuth2Authenticator_Completed;
            auth.Error += OAuth2Authenticator_Error;
        }

        public void New(object parameter, string accountStoreKeyName, string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl)
        {
            if (auth != null)
            {
                auth.Completed -= OAuth2Authenticator_Completed;
                auth.Error -= OAuth2Authenticator_Error;
            }

            this.accountStoreKeyName = accountStoreKeyName;

            if (!(parameter is Context))
                throw new ArgumentException("parameter must be a Context object");

            context = parameter as Context;

            LoadAccount();

            auth = new CustomOAuth2Authenticator(
                clientId: clientId,
                clientSecret: clientSecret,
                scope: scope,
                authorizeUrl: authorizeUrl,
                redirectUrl: redirectUrl,
                accessTokenUrl: accessTokenUrl);

            auth.Completed += OAuth2Authenticator_Completed;
            auth.Error += OAuth2Authenticator_Error;
        }

        private void LoadAccount()
        {
            IEnumerable<Account> accounts = AccountStore.Create(context).FindAccountsForService(accountStoreKeyName);
            if (accounts != null && accounts.Any())
                account = accounts.First();
            else
                account = null;
        }

        public IOAuth2Request CreateRequest(string method, Uri url, IDictionary<string, string> parameters, IAccount account)
        {
            var request = new CustomOAuth2Request(method, url, parameters, new Account(account.Username, account.Properties, account.Cookies));
            return new PlatformOAuth2Request(request);
        }

        public IOAuth2Request CreateRequest(string method, string accessTokenParameterName, Uri url, IDictionary<string, string> parameters, IAccount account)
        {
            var request = new CustomOAuth2Request(method, url, parameters, new Account(account.Username, account.Properties, account.Cookies));
            request.AccessTokenParameterName = accessTokenParameterName;
            return new PlatformOAuth2Request(request);
        }

        public void New(object parameter, string accountStoreKeyName, string clientId, string scope, Uri authorizeUrl, string loginRelativeUri, string tokenRelativeUri, Uri redirectUrl)
        {
            throw new NotImplementedException("Authorization code with Pkce is not implemented for this device");
        }

        #endregion
    }
}
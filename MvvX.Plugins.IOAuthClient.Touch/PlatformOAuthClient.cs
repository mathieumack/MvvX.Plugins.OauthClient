using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Touch
{
    public class PlatformOAuthClient : IOAuthClient
    {
        #region Fields

        private IAccount account;

        private OAuth2Authenticator auth;

        private object parameter;

        private UIViewController viewController;

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
            this.account = new PlatformAccount(e.Account);

            if (Completed != null)
            {
                this.Completed(sender, new PlatformAuthenticatorCompletedEventArgs(e));
            }
            viewController.DismissViewController(true, null);
        }

        public event EventHandler<IAuthenticatorErrorEventArgs> Error;

        private void OAuth2Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            if (Error != null)
            {
                this.Error(sender, new PlatformAuthenticatorErrorEventArgs(e));
            }
        }

        #endregion

        #region Methods

        public void Start(string title)
        {
            if (!(parameter is UIViewController))
                throw new ArgumentException("parameter must be a UIViewController object");

            viewController = auth.GetUI();
            (parameter as UIViewController).PresentViewController(viewController, true, null);

        }

        public void New(object parameter, string accountStoreKeyName, string clientId, string scope, Uri authorizeUrl, Uri redirectUrl)
        {
            if (auth != null)
            {
                auth.Completed -= OAuth2Authenticator_Completed;
                auth.Error -= OAuth2Authenticator_Error;
            }
            auth = new OAuth2Authenticator(
                clientId: clientId,
                scope: scope,
                authorizeUrl: authorizeUrl,
                redirectUrl: redirectUrl);

            auth.Completed += OAuth2Authenticator_Completed;
            auth.Error += OAuth2Authenticator_Error;

            this.parameter = parameter;
        }

        public void New(object parameter, string accountStoreKeyName, string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl)
        {
            if (auth != null)
            {
                auth.Completed -= OAuth2Authenticator_Completed;
                auth.Error -= OAuth2Authenticator_Error;
            }
            auth = new OAuth2Authenticator(
                clientId: clientId,
                clientSecret: clientSecret,
                scope: scope,
                authorizeUrl: authorizeUrl,
                redirectUrl: redirectUrl,
                accessTokenUrl: accessTokenUrl);

            auth.Completed += OAuth2Authenticator_Completed;
            auth.Error += OAuth2Authenticator_Error;

            this.parameter = parameter;
        }

        public IOAuth2Request CreateRequest(string method, Uri url, IDictionary<string, string> parameters, IAccount account)
        {
            var request = new OAuth2Request(method, url, parameters, new Account(account.Username, account.Properties, account.Cookies));
            return new PlatformOAuth2Request(request);
        }

        public IOAuth2Request CreateRequest(string method, string accessTokenParameterName, Uri url, IDictionary<string, string> parameters, IAccount account)
        {
            var request = new OAuth2Request(method, url, parameters, new Account(account.Username, account.Properties, account.Cookies));
            request.AccessTokenParameterName = accessTokenParameterName;
            return new PlatformOAuth2Request(request);
        }

        #endregion
    }
}
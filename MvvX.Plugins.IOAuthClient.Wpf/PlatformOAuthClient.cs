using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Navigation;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Wpf
{
    public class PlatformOAuthClient : IOAuthClient
    {
        #region Fields

        private Account account;

        public bool AuthorizationSuccess { get; set; }

        private CustomOAuth2Authenticator auth;
        private Window window;
        private OAuthLogonWebView webBrowserCtl;
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

        //public string AccessTokenName
        //{
        //    get
        //    {
        //        return auth.AccessTokenName;
        //    }

        //    set
        //    {
        //        auth.AccessTokenName = value;
        //    }
        //}

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

        //public bool DoNotEscapeScope
        //{
        //    get
        //    {
        //        return auth.DoNotEscapeScope;
        //    }

        //    set
        //    {
        //        auth.DoNotEscapeScope = value;
        //    }
        //}

        //public Dictionary<string, string> RequestParameters
        //{
        //    get
        //    {
        //        return auth.RequestParameters;
        //    }
        //}

        #endregion

        #region Events

        public event EventHandler<IAuthenticatorCompletedEventArgs> Completed;

        private void OAuth2Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            this.account = e.Account;

            if (Completed != null)
            {
                this.Completed(sender, new PlatformAuthenticatorCompletedEventArgs(e));
            }

            if (!AuthorizationSuccess)
            {
                // We need to show authrorization access page :
                webBrowserCtl.CheckAuthorization();
            }

            window.Close();
        }

        public event EventHandler<IAuthenticatorErrorEventArgs> Error;

        private void OAuth2Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            if (Error != null)
            {
                this.Error(sender, new PlatformAuthenticatorErrorEventArgs(e));
            }
            window.Close();
        }

        #endregion

        #region Methods

        public void Start(string screenTitle)
        {
            webBrowserCtl = new OAuthLogonWebView(auth);

            webBrowserCtl.Browser.Navigating += webBrowser_Navigating;
            webBrowserCtl.Browser.Navigated += webBrowser_Navigated;

            int maxWidth = 400;
            int maxHeight = 600;

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MvvX.Plugins.IOAuthClient.Wpf.Window.MaxWidth"]))
            {
                int settingsWidth;
                if (int.TryParse(ConfigurationManager.AppSettings["MvvX.Plugins.IOAuthClient.Wpf.Window.MaxWidth"], out settingsWidth))
                    maxWidth = settingsWidth;
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MvvX.Plugins.IOAuthClient.Wpf.Window.MaxHeight"]))
            {
                int settingsHeight;
                if (int.TryParse(ConfigurationManager.AppSettings["MvvX.Plugins.IOAuthClient.Wpf.Window.MaxHeight"], out settingsHeight))
                    maxHeight = settingsHeight;
            }

            window = new Window
            {
                Title = screenTitle,
                Content = webBrowserCtl,
                SizeToContent = SizeToContent.WidthAndHeight,
                MaxWidth = maxWidth,
                MaxHeight = maxHeight,
                Padding = new Thickness(0),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.CanResizeWithGrip,
                WindowStyle = (this.AllowCancel ? WindowStyle.ToolWindow : WindowStyle.None)
            };

            window.ShowDialog();
        }

        private void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            Console.WriteLine(">> webBrowser_Navigating : " + e.Uri.ToString());
            //auth.OnPageLoading(e.Uri);
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            Console.WriteLine(">> webBrowser_Navigated : " + e.Uri.ToString());
            auth.OnPageLoaded(e.Uri);
        }

        public void New(object parameter, string accountStoreKeyName, string clientId, string scope, Uri authorizeUrl, Uri redirectUrl)
        {
            if (auth != null)
            {
                auth.Completed -= OAuth2Authenticator_Completed;
                auth.Error -= OAuth2Authenticator_Error;
            }

            this.accountStoreKeyName = accountStoreKeyName;

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
            //IEnumerable<Account> accounts = AccountStore.Create().FindAccountsForService(accountStoreKeyName);
            //if (accounts != null && accounts.Any())
            //    account = accounts.First();
            //else
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

        #endregion
    }
}
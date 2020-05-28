using System;
using System.Windows;
using System.Windows.Controls;

namespace MvvX.Plugins.IOAuthClient
{
    /// <summary>
    /// Interaction logic for OAuthLogonWebView.xaml
    /// </summary>
    public partial class OAuthLogonWebView : UserControl
    {
        private readonly OAuth2AuthenticatorBase platformOAuthClient;

        public WebBrowser Browser
        {
            get
            {
                return webBrowser;
            }
        }

        public Loader Loader
        {
            get
            {
                return loader;
            }
        }

        public OAuthLogonWebView(OAuth2AuthenticatorBase platformOAuthClient)
        {
            this.platformOAuthClient = platformOAuthClient;
            this.platformOAuthClient.TokenAccessReceived -= OnTokenAccessReceived;
            this.platformOAuthClient.TokenAccessReceived += OnTokenAccessReceived;

            InitializeComponent();

            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                //Add the message hook in the code behind since I got a weird bug when trying to do it in the XAML
                //webBrowser.MessageHook += WebBrowser_MessageHook;

                //Delete the cookies since the last authentication
                //DeleteCookies();

                //Create the destination URL
                var getUriTask = platformOAuthClient.GetInitialUrlAsync();
                getUriTask.Wait();

                var destinationURL = getUriTask.Result;
                webBrowser.Navigate(destinationURL);
            };
        }

        private void OnTokenAccessReceived(object sender, EventArgs e)
        {
            if (sender is OAuth2AuthenticatorBase)
            {
                this.webBrowser.Visibility = Visibility.Hidden;
                this.loader.Visibility = Visibility.Visible;
            }
        }

        //private System.IntPtr WebBrowser_MessageHook(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
        //{
        //   // throw new System.NotImplementedException();
        //}
    }
}

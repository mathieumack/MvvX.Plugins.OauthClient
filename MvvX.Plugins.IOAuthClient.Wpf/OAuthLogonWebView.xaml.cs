using System.Windows;
using System.Windows.Controls;

namespace MvvX.Plugins.IOAuthClient.Wpf
{
    /// <summary>
    /// Interaction logic for OAuthLogonWebView.xaml
    /// </summary>
    public partial class OAuthLogonWebView : UserControl
    {
        private readonly CustomOAuth2Authenticator platformOAuthClient;

        public WebBrowser Browser
        {
            get
            {
                return webBrowser;
            }
        }

        public OAuthLogonWebView(CustomOAuth2Authenticator platformOAuthClient)
        {
            this.platformOAuthClient = platformOAuthClient;
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

        internal void CheckAuthorization()
        {
            //Create the destination URL
            var getUriTask = platformOAuthClient.GetAuthorizationUrlAsync();
            getUriTask.Wait();

            var destinationURL = getUriTask.Result;
            webBrowser.Navigate(destinationURL);
        }

        //private System.IntPtr WebBrowser_MessageHook(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
        //{
        //   // throw new System.NotImplementedException();
        //}
    }
}

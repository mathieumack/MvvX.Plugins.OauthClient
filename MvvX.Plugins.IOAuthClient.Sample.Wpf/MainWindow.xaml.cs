using MvvX.Plugins.IOAuthClient.Wpf;
using System;
using System.Windows;

namespace MvvX.Plugins.IOAuthClient.Sample.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            IOAuthClient auth = new PlatformOAuthClient();

            auth.New(this,
                        "temporaryKey",
                        "<client_id>",
                        "<client_secret>",
                        "",
                        new Uri("<authorization_uri>"),
                        new Uri("<redirect_uri>"),
                        new Uri("<token_uri>"));

            auth.AllowCancel = true;

            //auth.Error += (s, ee) =>
            //{
            //    Console.WriteLine(ee.Message);
            //};

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) =>
            {
                if (!ee.IsAuthenticated)
                {
                    MessageBox.Show("Not Authenticated");
                    return;
                }

                else
                {
                    MessageBox.Show("Authenticated, well done");
                    return;
                }
            };

            auth.Start("Login");
        }
    }
}

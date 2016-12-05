using System;
using Xamarin.Auth;

namespace MvvX.Plugins.Xamarin.Auth.Wpf
{
    public class PlatformXamarinAuth
    {
        private bool isloggingin;

        private void LoginToFacebook()
        {
            isloggingin = true;
            var auth = new OAuth2Authenticator(
                clientId: "548850318570514",
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += LoginComplete;
            
            var iMvxAndroidGlobals = Mvx.Resolve<IMvxAndroidGlobals>();
            iMvxAndroidGlobals.ApplicationContext.StartActivity(auth.GetUI(iMvxAndroidGlobals.ApplicationContext));
        }

        public void LoginComplete(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (!e.IsAuthenticated)
            {
                Console.WriteLine("Not Authorised");
                return;
            }

            var model = new ModelFacebook();

            var accessToken = e.Account.Properties["access_token"].ToString();
            var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
            var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);

            // Now that we're logged in, make a OAuth2 request to get the user's id.
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, e.Account);

            request.GetResponseAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                    model.Hello = "Error: " + t.Exception.InnerException.Message;
                else
                {
                    string mail = t.Result.GetResponseText();

                    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(mail);
                    string email;
                    bool s = values.TryGetValue("email", out email);
                    if (s)
                    {
                        model.Hello = email;
                    }
                    else
                        model.Hello = "Error encountered at the absolutely last second";
                }
            });
        }
    }
}

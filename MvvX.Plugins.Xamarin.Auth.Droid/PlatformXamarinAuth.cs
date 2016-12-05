using System;
using System.Collections.Generic;
using Xamarin.Auth;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Android.App;
using System.Json;
using System.Threading.Tasks;

namespace MvvX.Plugins.Xamarin.Auth.Droid
{
    internal class ModelFacebook
    {
        public string Hello { get; set; }
    }

    public class PlatformXamarinAuth
    {
        private void Authenticate()
        {
            var auth = new OAuth2Authenticator(
                   clientId: "App ID from https://developers.facebook.com/apps",
                   scope: "",
                   authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                   redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            //auth.AllowCancel = allowCancel;

            var iMvxAndroidGlobals = Mvx.Resolve<IMvxAndroidGlobals>();

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) => {

                if (!ee.IsAuthenticated)
                {
                    var builder = new AlertDialog.Builder(iMvxAndroidGlobals.ApplicationContext);
                    builder.SetMessage("Not Authenticated");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
                request.GetResponseAsync().ContinueWith(t => {
                    var builder = new AlertDialog.Builder(iMvxAndroidGlobals.ApplicationContext);
                    if (t.IsFaulted)
                    {
                        builder.SetTitle("Error");
                        builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    }
                    else if (t.IsCanceled)
                        builder.SetTitle("Task Canceled");
                    else
                    {
                        var obj = JsonValue.Parse(t.Result.GetResponseText());

                        builder.SetTitle("Logged in");
                        builder.SetMessage("Name: " + obj["name"]);
                    }

                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }, UIScheduler);
            };

            var intent = auth.GetUI(iMvxAndroidGlobals.ApplicationContext);
            iMvxAndroidGlobals.ApplicationContext.StartActivity(intent);
        }

        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        //private bool isloggingin;

        //private void LoginToFacebook()
        //{
        //    isloggingin = true;
        //    var auth = new OAuth2Authenticator(
        //        clientId: "548850318570514",
        //        scope: "email",
        //        authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
        //        redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

        //    // If authorization succeeds or is canceled, .Completed will be fired.
        //    auth.Completed += LoginComplete;

        //    var iMvxAndroidGlobals = Mvx.Resolve<IMvxAndroidGlobals>();
        //    iMvxAndroidGlobals.ApplicationContext.StartActivity(auth.GetUI(iMvxAndroidGlobals.ApplicationContext));
        //}

        //public void LoginComplete(object sender, AuthenticatorCompletedEventArgs e)
        //{
        //    if (!e.IsAuthenticated)
        //    {
        //        Console.WriteLine("Not Authorised");
        //        return;
        //    }

        //    var model = new ModelFacebook();

        //    var accessToken = e.Account.Properties["access_token"].ToString();
        //    var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
        //    var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);

        //    // Now that we're logged in, make a OAuth2 request to get the user's id.
        //    var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, e.Account);

        //    request.GetResponseAsync().ContinueWith(t =>
        //    {
        //        if (t.IsFaulted)
        //            model.Hello = "Error: " + t.Exception.InnerException.Message;
        //        else
        //        {
        //            string mail = t.Result.GetResponseText();

        //            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(mail);
        //            string email;
        //            bool s = values.TryGetValue("email", out email);
        //            if (s)
        //            {
        //                model.Hello = email;
        //            }
        //            else
        //                model.Hello = "Error encountered at the absolutely last second";
        //        }
        //    });
        //}
    }
}
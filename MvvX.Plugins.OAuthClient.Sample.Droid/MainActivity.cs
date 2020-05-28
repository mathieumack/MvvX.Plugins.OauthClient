using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;

namespace MvvX.Plugins.OAuthClient.Sample.Droid
{
    [Activity(Label = "MvvX.Plugins.OAuthClient.Sample.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        void Login(bool allowCancel)
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

            auth.Error += (s, ee) =>
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetMessage(ee.Message);
                builder.SetPositiveButton("Ok", (o, e) => { });
                builder.Create().Show();
                return;
            };

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) =>
            {
                if (!ee.IsAuthenticated)
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage("Not Authenticated");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                else
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage("Authenticated, well done");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }
            };

            auth.Start("Xamarin.Auth login");
        }

        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var facebook = FindViewById<Button>(Resource.Id.FacebookButton);
            facebook.Click += delegate { Login(true); };

            var facebookNoCancel = FindViewById<Button>(Resource.Id.FacebookButtonNoCancel);
            facebookNoCancel.Click += delegate { Login(false); };
        }
    }
}


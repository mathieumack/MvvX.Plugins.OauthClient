using Android.App;
using Android.Widget;
using Android.OS;
using MvvX.Plugins.IOAuthClient.Droid;
using System;
using System.Threading.Tasks;
using System.Json;

namespace MvvX.Plugins.IOAuthClient.Sample.Droid
{
    [Activity(Label = "MvvX.Plugins.IOAuthClient.Sample.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        void Login(bool allowCancel)
        {
            IOAuthClient auth = new PlatformOAuthClient();
            
            auth.AllowCancel = allowCancel;

            auth.Error += (s, ee) =>
            {
                Console.WriteLine(ee.Message);
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
                
                //Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = auth.RefreshToken(new Uri("<set_uri>"));
                request.GetResponseAsync().ContinueWith(t =>
                {
                    var builder = new AlertDialog.Builder(this);
                    if (t.IsFaulted)
                    {
                        Console.WriteLine("t.faulted");
                        builder.SetTitle("Error");
                        builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    }
                    else if (t.IsCanceled)
                    {
                        Console.WriteLine("t.IsCanceled");
                        builder.SetTitle("Task Canceled");
                    }
                    else
                    {
                        Console.WriteLine("t.IsCanceled");
                        var obj = JsonValue.Parse(t.Result.GetResponseText());

                        builder.SetTitle("Request :");
                        builder.SetMessage(t.Result.GetResponseText());
                    }

                    Console.WriteLine(t.Result.GetResponseText());

                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }, UIScheduler);
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


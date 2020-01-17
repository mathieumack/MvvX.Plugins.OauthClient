using System;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using MvvX.Plugins.IOAuthClient.Droid;

namespace MvvX.Plugins.IOAuthClient.Sample.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        void Login()
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
                var builder = new Android.App.AlertDialog.Builder(this);
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
                    var builder = new Android.App.AlertDialog.Builder(this);
                    builder.SetMessage("Not Authenticated");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                else
                {
                    var builder = new Android.App.AlertDialog.Builder(this);
                    builder.SetMessage("Authenticated, well done");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }
            };

            auth.Start("Xamarin.Auth login");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            Login();
        }
    }
}

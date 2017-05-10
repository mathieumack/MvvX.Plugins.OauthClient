using System;
using System.Json;
using System.Threading.Tasks;
using Foundation;
using MonoTouch.Dialog;
using MvvX.Plugins.IOAuthClient.Touch;
using UIKit;

namespace MvvX.Plugins.IOauthClient.Sample.Touch
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        Section facebook;
        StringElement facebookStatus;
        DialogViewController dialog;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            // If you have defined a root view controller, set it here:
            // Window.RootViewController = myViewController;

            facebook = new Section("Facebook");
            facebook.Add(new StyledStringElement("Log in", () => LoginToFacebook(true)));
            facebook.Add(new StyledStringElement("Log in (no cancel)", () => LoginToFacebook(false)));
            facebook.Add(facebookStatus = new StringElement(String.Empty));

            dialog = new DialogViewController(new RootElement("Xamarin.Auth Sample") {
                facebook,
            });

            Window.RootViewController = new UINavigationController(dialog);
            Window.MakeKeyAndVisible();

            return true;
        }

        void LoginToFacebook(bool allowCancel)
        {
            IOAuthClient.IOAuthClient auth = new PlatformOAuthClient();

            auth.New(dialog,
                        "temporaryKey",
                        clientId: "App ID from https://developers.facebook.com/apps",
                        scope: "",
                        authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                        redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            auth.AllowCancel = allowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, e) =>
            {
                // We presented the UI, so it's up to us to dismiss it.
                dialog.DismissViewController(true, null);

                if (!e.IsAuthenticated)
                {
                    facebookStatus.Caption = "Not authorized";
                    dialog.ReloadData();
                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = auth.CreateRequest("GET", new Uri("https://graph.facebook.com/me"), null, e.Account);
                request.GetResponseAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        facebookStatus.Caption = "Error: " + t.Exception.InnerException.Message;
                    else if (t.IsCanceled)
                        facebookStatus.Caption = "Canceled";
                    else
                    {
                        var obj = JsonValue.Parse(t.Result.GetResponseText());
                        facebookStatus.Caption = "Logged in as " + obj["name"];
                    }

                    dialog.ReloadData();
                }, uiScheduler);
            };

            auth.Start("Facebook login");
        }

        private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}



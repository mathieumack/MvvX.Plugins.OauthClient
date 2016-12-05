using System;
using System.Collections.Generic;
using Xamarin.Auth;
using MvvmCross.Platform.Touch;
using MvvmCross.Platform;
using Newtonsoft.Json;

namespace MvvX.Plugins.Xamarin.Auth.Touch
{
    internal class ModelFacebook
    {
        public string Hello { get; set; }
    }

    public class PlatformXamarinAuth
    {
        void LoginToFacebook(bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
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
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, e.Account);
                request.GetResponseAsync().ContinueWith(t => {
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

            UIViewController vc = auth.GetUI();
            dialog.PresentViewController(vc, true, null);
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            facebook = new Section("Facebook");
            facebook.Add(new StyledStringElement("Log in", () => LoginToFacebook(true)));
            facebook.Add(new StyledStringElement("Log in (no cancel)", () => LoginToFacebook(false)));
            facebook.Add(facebookStatus = new StringElement(String.Empty));

            dialog = new DialogViewController(new RootElement("Xamarin.Auth Sample") {
                facebook,
            });

            window = new UIWindow(UIScreen.MainScreen.Bounds);
            window.RootViewController = new UINavigationController(dialog);
            window.MakeKeyAndVisible();

            return true;
        }

        private readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        UIWindow window;
        DialogViewController dialog;

        Section facebook;
        StringElement facebookStatus;
    }
}
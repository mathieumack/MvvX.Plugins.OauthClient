using System;
using System.Collections.Generic;

namespace MvvX.Plugins.IOAuthClient
{
    public interface IOAuthClient
    {
        bool AllowCancel { get; set; }

        string AccessTokenName { get; set; }

        string ClientId { get; }

        string ClientSecret { get; }

        bool DoNotEscapeScope { get; set; }

        Dictionary<string, string> RequestParameters { get; }

        event EventHandler<IAuthenticatorCompletedEventArgs> Completed;

        /// <summary>
        /// Configuration object.
        /// For Android : Set an Activity, for iOS : Set a Section object that is the parent object
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="title"></param>
        void Start(object parameter, string title);

        void New(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl); //, GetUsernameAsyncFunc getUsernameAsync = null);

        void New(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl); //, GetUsernameAsyncFunc getUsernameAsync = null);

        IOAuth2Request CreateRequest(string method, Uri url, IDictionary<string, string> parameters, IAccount account);
    }
}

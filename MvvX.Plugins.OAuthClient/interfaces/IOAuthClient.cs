using System;
using System.Collections.Generic;

namespace MvvX.Plugins.OAuthClient
{
    public interface IOAuthClient
    {
        bool AllowCancel { get; set; }

        string ClientId { get; }

        event EventHandler<IAuthenticatorCompletedEventArgs> Completed;

        event EventHandler<IAuthenticatorErrorEventArgs> Error;

        /// <summary>
        /// Configuration object.
        /// </summary>
        /// <param name="screenTitle"></param>
        void Start(string screenTitle);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">For Android : Set an Activity, for iOS : Set a Section object that is the parent object</param>
        /// <param name="accountStoreKeyName"></param>
        /// <param name="clientId"></param>
        /// <param name="scope"></param>
        /// <param name="authorizeUrl"></param>
        /// <param name="redirectUrl"></param>
        void New(object parameter, string accountStoreKeyName, string clientId, string scope, Uri authorizeUrl, Uri redirectUrl);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">For Android : Set an Activity, for iOS : Set a Section object that is the parent object</param>
        /// <param name="accountStoreKeyName"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scope"></param>
        /// <param name="authorizeUrl"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="accessTokenUrl"></param>
        void New(object parameter, string accountStoreKeyName, string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl);

        /// <summary>
        /// Instanciates a PKCE flow authentication
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="accountStoreKeyName">Name of the account store key.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="authorizeUrl">The authorize URL.</param>
        /// <param name="loginRelativeUri">The login relative URI.</param>
        /// <param name="tokenRelativeUri">The token relative URI.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        void New(object parameter, string accountStoreKeyName, string clientId, string scope, Uri authorizeUrl, string loginRelativeUri, string tokenRelativeUri, Uri redirectUrl);

        IOAuth2Request CreateRequest(string method, Uri url, IDictionary<string, string> parameters, IAccount account);

        IOAuth2Request CreateRequest(string method, string accessTokenParameterName, Uri url, IDictionary<string, string> parameters, IAccount account);
    }
}

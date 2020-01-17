using System;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Droid
{
    /// <summary>
    /// Custom authenticator
    /// </summary>
    public class CustomOAuth2Authenticator : OAuth2Authenticator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="scope"></param>
        /// <param name="authorizeUrl"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="getUsernameAsync"></param>
        /// <param name="ignoreErrorsWhenCompleted"></param>
        public CustomOAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, bool ignoreErrorsWhenCompleted, GetUsernameAsyncFunc getUsernameAsync = null)
            : base(clientId, scope, authorizeUrl, redirectUrl, getUsernameAsync)
        {
            IgnoreErrorsWhenCompleted = ignoreErrorsWhenCompleted;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="scope"></param>
        /// <param name="authorizeUrl"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="accessTokenUrl"></param>
        /// <param name="getUsernameAsync"></param>
        /// <param name="ignoreErrorsWhenCompleted"></param>
        public CustomOAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, bool ignoreErrorsWhenCompleted, GetUsernameAsyncFunc getUsernameAsync = null)
            : base(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, getUsernameAsync)
        {
            IgnoreErrorsWhenCompleted = ignoreErrorsWhenCompleted;
        }
    }
}
using System;
using Xamarin.Auth;

namespace MvvX.Plugins.OAuthClient
{
    /// <summary>
    /// Represents a base class for OAuth2 authenticators
    /// </summary>
    /// <seealso cref="Xamarin.Auth.WebRedirectAuthenticator" />
    public class OAuth2AuthenticatorBase : WebRedirectAuthenticator
    {
        public event EventHandler TokenAccessReceived;

        /// <summary>
        /// Gets the redirect URL.
        /// </summary>
        /// <value>The redirect URL.</value>
        public Uri RedirectUrl
        {
            get; protected set;
        }

        public string ClientId
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the authorization scope.
        /// </summary>
        /// <value>The authorization scope.</value>
        public string Scope
        {
            get; protected set;
        }

        /// <summary>
        /// Gets the authorize URL.
        /// </summary>
        /// <value>The authorize URL.</value>
        public Uri BaseUri
        {
            get; protected set;
        }

        public OAuth2AuthenticatorBase(string clientId, string scope, Uri baseUri, Uri redirectUri)
            : base(baseUri, redirectUri)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId), "clientId must be provided");
            }

            if (baseUri == default(Uri))
            {
                throw new ArgumentNullException(nameof(baseUri), "Initial url must be provided");
            }

            if (redirectUri == default(Uri))
            {
                throw new ArgumentNullException(nameof(redirectUri), "reditect url must be provided");
            }

            BaseUri = baseUri;
            ClientId = clientId;
            RedirectUrl = redirectUri;
            Scope = scope ?? "";
        }

        protected void OnTokenReceived(EventArgs e)
        {
            TokenAccessReceived?.Invoke(this, e);
        }
    }
}
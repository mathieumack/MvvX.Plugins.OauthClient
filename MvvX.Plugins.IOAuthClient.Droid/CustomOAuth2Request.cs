using System;
using System.Collections.Generic;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;

namespace MvvX.Plugins.IOAuthClient.Droid
{
    public class CustomOAuth2Request : OAuth2Request
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Request"/> class.
        /// </summary>
        /// <param name='method'>The HTTP method.</param>
        /// <param name='url'>The URL.</param>
        /// <param name='parameters'>
        /// Parameters that will pre-populate the <see cref="Request.Parameters"/> property or <c>null</c>.
        /// </param>
        /// <param name='account'>The account used to authenticate this request.</param>
        public CustomOAuth2Request(string method, Uri url, IDictionary<string, string> parameters, Account account)
			: base (method, url, parameters, account)
		{
        }


        /// <summary>
        /// Asynchronously gets the response.
        /// </summary>
        /// <returns>
        /// The response.
        /// </returns>
        /// <exception cref="InvalidOperationException"><see cref="Account"/> is <c>null</c>.</exception>
        public override Task<Response> GetResponseAsync(CancellationToken cancellationToken)
        {
            //
            // Make sure we have an account
            //
            if (Account == null)
            {
                throw new InvalidOperationException("You must specify an Account for this request to proceed");
            }

            //
            // Sign the request before getting the response
            //
            var req = GetPreparedWebRequest();

            //
            // Authorize it
            //
            var authorization = GetAuthorizationHeader(Account);
            
            req.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);

            return base.GetResponseAsync(cancellationToken);
        }

        ///// <summary>
        ///// Gets an authenticated HTTP Authorization header.
        ///// </summary>
        ///// <returns>
        ///// The authorization header.
        ///// </returns>
        ///// <param name='account'>The <see cref="Account"/> that's been authenticated.</param>
        //public static string GetAuthorizationHeaderBasic(Account account)
        //{
        //    if (account == null)
        //    {
        //        throw new ArgumentNullException("account");
        //    }
        //    if (!account.Properties.ContainsKey("access_token"))
        //    {
        //        throw new ArgumentException("OAuth2 account is missing required access_token property.", "account");
        //    }

        //    return "Basic " + account.Properties["access_token"];
        //}
    }
}
using System;
using System.Collections.Generic;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Headers;
using System.IO;

namespace MvvX.Plugins.OAuthClient
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

            var parameters = Parameters.FormEncode();
            Console.WriteLine("Parameters : " + parameters);
            //
            // Authorize it
            //
            var authorization = GetAuthorizationHeaderBasic(Account);

            Console.WriteLine("GetResponseAsync : AuthorizationHeader - " + authorization);

            req.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);

            return base.GetResponseAsync(cancellationToken);
        }

        public override void AddMultipartData(string name, Stream data, string mimeType = "", string filename = "")
        {
            Console.WriteLine("AddMultipartData : name - " + name);
            base.AddMultipartData(name, data, mimeType, filename);
        }

  //      /// <summary>
		///// Gets the response.
		///// </summary>
		///// <remarks>
		///// Service implementors should override this method to modify the PreparedWebRequest
		///// to authenticate it.
		///// </remarks>
		///// <param name="cancellationToken"></param>
		///// <returns>
		///// The response.
		///// </returns>
		//public override Task<Response> GetResponseAsync(CancellationToken cancellationToken)
  //      {
  //          var request = GetPreparedWebRequest();

  //          //
  //          // Authorize it
  //          //
  //          var authorization = GetAuthorizationHeaderBasic(Account);

  //          Mvx.Trace("GetResponseAsync : AuthorizationHeader - " + authorization);

  //          request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorization);

  //          //
  //          // Disable 100-Continue: http://blogs.msdn.com/b/shitals/archive/2008/12/27/9254245.aspx
  //          //
  //          if (Method == "POST")
  //          {
  //              ServicePointManager.Expect100Continue = false;
  //          }

  //          if (Multiparts.Count > 0)
  //          {
  //              var boundary = "---------------------------" + new Random().Next();
  //              request.ContentType = "multipart/form-data; boundary=" + boundary;

  //              return Task.Factory
  //                      .FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null)
  //                      .ContinueWith(reqStreamtask => {

  //                          using (reqStreamtask.Result)
  //                          {
  //                              WriteMultipartFormData(boundary, reqStreamtask.Result);
  //                          }

  //                          return Task.Factory
  //                                          .FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
  //                                          .ContinueWith(resTask => {
  //                                              return new Response((HttpWebResponse)resTask.Result);
  //                                          }, cancellationToken);
  //                      }, cancellationToken).Unwrap();
  //          }
  //          else if (Method == "POST" && Parameters.Count > 0)
  //          {
  //              var body = Parameters.FormEncode();
  //              var bodyData = System.Text.Encoding.UTF8.GetBytes(body);
  //              request.ContentLength = bodyData.Length;
  //              request.ContentType = "application/x-www-form-urlencoded";

  //              return Task.Factory
  //                      .FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null)
  //                      .ContinueWith(reqStreamTask => {

  //                          using (reqStreamTask.Result)
  //                          {
  //                              reqStreamTask.Result.Write(bodyData, 0, bodyData.Length);
  //                          }

  //                          return Task.Factory
  //                                      .FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
  //                                          .ContinueWith(resTask => {
  //                                              return new Response((HttpWebResponse)resTask.Result);
  //                                          }, cancellationToken);
  //                      }, cancellationToken).Unwrap();
  //          }
  //          else
  //          {
  //              return Task.Factory
  //                      .FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
  //                      .ContinueWith(resTask => {
  //                          return new Response((HttpWebResponse)resTask.Result);
  //                      }, cancellationToken);
  //          }
  //      }

        /// <summary>
        /// Gets an authenticated HTTP Authorization header.
        /// </summary>
        /// <returns>
        /// The authorization header.
        /// </returns>
        /// <param name='account'>The <see cref="Account"/> that's been authenticated.</param>
        public static string GetAuthorizationHeaderBasic(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (!account.Properties.ContainsKey("access_token"))
            {
                throw new ArgumentException("OAuth2 account is missing required access_token property.", "account");
            }

            return "Basic " + account.Properties["access_token"];
        }
    }
}
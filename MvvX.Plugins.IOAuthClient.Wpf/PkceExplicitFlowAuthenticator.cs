using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Utilities;

namespace MvvX.Plugins.IOAuthClient.Wpf
{
    public class PkceExplicitFlowAuthenticator : OAuth2AuthenticatorBase
    {
        public const string ChallengeMethod = "S256";
        public const string LoginUrlPattern = "{0}/{1}?client_id={2}&response_type=code&redirect_uri={3}&code_challenge={4}&code_challenge_method={5}";

        private string challenge;
        private string verifier;
        private readonly string loginRelativeUri;
        private readonly string tokenRelativeUri;

        #region Constructor

        public PkceExplicitFlowAuthenticator(string clientId, string scope, Uri baseUri, string loginRelativeUri, string tokenRelativeUri, Uri redirectUrl)
            : base(clientId, scope, baseUri, redirectUrl)
        {
            if (string.IsNullOrEmpty(loginRelativeUri))
            {
                throw new ArgumentNullException(nameof(loginRelativeUri), "loginRelativeUri must be provided");
            }

            if (string.IsNullOrEmpty(tokenRelativeUri))
            {
                throw new ArgumentNullException(nameof(tokenRelativeUri), "tokenRelativeUri must be provided");
            }

            this.loginRelativeUri = loginRelativeUri;
            this.tokenRelativeUri = tokenRelativeUri;
            InitializeProtocolParameters();
        }

        #endregion Constructor

        #region Public methods

        public override async Task<Uri> GetInitialUrlAsync()
        {
            return await Task.FromResult(new Uri(string.Format(LoginUrlPattern, BaseUri.AbsoluteUri, loginRelativeUri, ClientId, RedirectUrl, challenge, ChallengeMethod)));
        }


        #endregion Public methods

        #region Private methods

        private void InitializeProtocolParameters()
        {
            GenerateCodeVerifier();
            CreateChallengeCode();
        }

        private void GenerateCodeVerifier()
        {
            var buffer = new byte[32];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            verifier = Convert.ToBase64String(buffer).Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        private void CreateChallengeCode()
        {
            var sha = new SHA256Managed();
            sha.ComputeHash(Encoding.UTF8.GetBytes(verifier));
            challenge = Convert.ToBase64String(sha.Hash).Replace('+', '-').Replace('/', '_').Replace("=", "");
        }


        protected virtual Uri GetTokenUrl()
        {
            return new Uri($"{BaseUri.AbsoluteUri}/{this.tokenRelativeUri}");
        }

        #endregion Private methods

        protected override void OnRedirectPageLoaded(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            if (query.ContainsKey("code"))
            {
                var autorizationCode = query["code"];
                var tokenUrl = GetTokenUrl();
                try
                {
                    using (var client = new HttpClient())
                    {
                        var postBody = new Dictionary<string, string>();
                        postBody.Add("grant_type", "authorization_code");
                        postBody.Add("client_id", this.ClientId);
                        postBody.Add("code_verifier", this.verifier);
                        postBody.Add("redirect_uri", this.RedirectUrl.AbsoluteUri);
                        postBody.Add("code", autorizationCode);
                        var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);
                        request.Content = new FormUrlEncodedContent(postBody);
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                        var response = client.SendAsync(request).Result;
                        OnTokenReceived(new EventArgs());
                        response.EnsureSuccessStatusCode();
                        var res = response.Content.ReadAsStringAsync().Result;
                        var data = res.Contains("{") ? WebEx.JsonDecode(res) : WebEx.FormDecode(res);
                        OnSucceeded(string.Empty, data);
                    }
                }
                catch
                {
                    OnError("An unexpected error occured while getting access token.");
                }
            }
            else
            {
                OnError("The expected authorization code not received");
            }

            base.OnRedirectPageLoaded(url, query, fragment);
        }
    }
}
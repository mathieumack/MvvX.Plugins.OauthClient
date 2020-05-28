using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient
{
    public class PlatformOAuth2Request : IOAuth2Request
    {
        #region Fields

        private readonly OAuth2Request request;

        #endregion

        #region Constructor

        public PlatformOAuth2Request(OAuth2Request request)
        {
            this.request = request;
        }

        public async Task<IResponse> GetResponseAsync()
        {
            var response = await request.GetResponseAsync();
            return new PlatformResponse(response);
        }

        public async Task<IResponse> GetResponseAsync(CancellationToken cancellationToken)
        {
            var response = await request.GetResponseAsync(cancellationToken);
            return new PlatformResponse(response);
        }

        #endregion
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Wpf
{
    public class PlatformOAuth1Request : IOAuth2Request
    {
        #region Fields

        private readonly OAuth1Request request;

        #endregion

        #region Constructor

        public PlatformOAuth1Request(OAuth1Request request)
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace MvvX.Plugins.IOAuthClient.Wpf
{
    public class PlatformResponse : IResponse
    {
        #region Fields

        private readonly Response response;

        public IDictionary<string, string> Headers
        {
            get
            {
                return response.Headers;
            }
        }

        public Uri ResponseUri
        {
            get
            {
                return response.ResponseUri;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return response.StatusCode;
            }
        }

        #endregion

        #region Constructor

        public PlatformResponse(Response response)
        {
            this.response = response;
        }

        #endregion

        #region Public methods

        public void Dispose()
        {
            response.Dispose();
        }

        public Stream GetResponseStream()
        {
            return response.GetResponseStream();
        }

        public async Task<Stream> GetResponseStreamAsync()
        {
            return await response.GetResponseStreamAsync();
        }

        public string GetResponseText()
        {
            return response.GetResponseText();
        }

        public async Task<string> GetResponseTextAsync()
        {
            return await response.GetResponseTextAsync();
        }

        #endregion
    }
}
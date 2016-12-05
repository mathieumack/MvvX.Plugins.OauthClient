using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MvvX.Plugins.IOAuthClient
{
    public interface IResponse : IDisposable
    {
        IDictionary<string, string> Headers { get; }

        Uri ResponseUri { get; }

        HttpStatusCode StatusCode { get;  }
        
        Stream GetResponseStream();

        Task<Stream> GetResponseStreamAsync();

        string GetResponseText();

        Task<string> GetResponseTextAsync();
    }
}

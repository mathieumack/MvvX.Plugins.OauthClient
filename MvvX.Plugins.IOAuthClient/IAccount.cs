using System.Collections.Generic;
using System.Net;

namespace MvvX.Plugins.IOAuthClient
{
    public interface IAccount
    {
        CookieContainer Cookies { get; }

        Dictionary<string, string> Properties { get; }

        string Username { get; set; }
        
        string Serialize();
    }
}

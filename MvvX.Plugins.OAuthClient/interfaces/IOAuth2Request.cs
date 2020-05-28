using System.Threading;
using System.Threading.Tasks;

namespace MvvX.Plugins.OAuthClient
{
    public interface IOAuth2Request
    {
        Task<IResponse> GetResponseAsync();

        Task<IResponse> GetResponseAsync(CancellationToken cancellationToken);
    }
}

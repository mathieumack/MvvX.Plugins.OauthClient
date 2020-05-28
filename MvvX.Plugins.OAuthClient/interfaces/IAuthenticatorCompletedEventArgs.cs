namespace MvvX.Plugins.OAuthClient
{
    public interface IAuthenticatorCompletedEventArgs
    {
        IAccount Account { get; }

        bool IsAuthenticated { get; }
    }
}

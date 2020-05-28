namespace MvvX.Plugins.IOAuthClient
{
    public interface IAuthenticatorCompletedEventArgs
    {
        IAccount Account { get; }

        bool IsAuthenticated { get; }
    }
}

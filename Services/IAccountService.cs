using Kviz.Model;

namespace Kviz.Services
{
    public interface IAccountService
    {
        Task<(bool flag, string message)> RegisterAsync(User user);
        Task<string> LoginAsync(Login model);
    }
}

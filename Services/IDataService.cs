using Kviz.Model;

namespace Kviz.Services
{
    public interface IDataService
    {
        public Task<List<User>> GetUsers();
    }
}

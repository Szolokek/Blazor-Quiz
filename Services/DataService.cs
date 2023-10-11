using Kviz.Data;
using Kviz.Model;
namespace Kviz.Services
{

    public class DataService : IDataService
    { 
        
        private DataContext _context;

        public DataService(DataContext context)
        {
            
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}

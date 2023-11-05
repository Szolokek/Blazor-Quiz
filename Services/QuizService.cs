using Kviz.Data;
using Kviz.Migrations;

namespace Kviz.Services
{
    public class QuizService
    {
        public int SessionId { get; set; }
        public Dictionary<int, GameService> Sessions { get; set; }
        private readonly IDataService _dataService;
        public QuizService(IDataService dataService) 
        {
            _dataService = dataService;
            Sessions = new Dictionary<int, GameService>(); 
        }

        public void AddNewSession(int sessionId)
        {
            Sessions.Add(sessionId, new GameService(_dataService));
        }
    }
}

using Kviz.Data;
using Kviz.Migrations;
using System.Collections.Concurrent;

namespace Kviz.Services
{
    public class QuizService
    {
        public int SessionId { get; set; }
        public ConcurrentDictionary<int, GameService> Sessions { get; set; }
        public QuizService() 
        {
            Sessions = new ConcurrentDictionary<int, GameService>(); 
        }

        public void AddNewSession(int sessionId)
        {
            Sessions.TryAdd(sessionId, new GameService());
        }
    }
}

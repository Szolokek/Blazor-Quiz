namespace Kviz.Services
{
    public class QuizService
    {
        public int SessionId { get; set; }
        public Dictionary<int, GameService> Sessions { get; set; }

        public QuizService() 
        {
            Sessions = new Dictionary<int, GameService>(); 
        }
    }
}

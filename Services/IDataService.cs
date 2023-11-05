using Kviz.Migrations.Tables;
using Kviz.Model;

namespace Kviz.Services
{
    public interface IDataService
    {
        public Task<List<QuizTable>> GetQuizes();
        public void CreateNewQuiz(Quiz quiz);
        public Task<Quiz> GetQuizByIdAsync(int Id);
        public void UpdateQuiz(Quiz quiz, int Id);
        public Task<int> CreateSession(int Id);
        public Task<List<SessionTable>> GetSessions();
        public void SetSessionDate(int Id);
        public Task<SessionTable> GetSessionById(int Id);
        public void SaveToHistory(Dictionary<Answer, List<string>> userAnswers, int questionId, int sessionId);
    }
}

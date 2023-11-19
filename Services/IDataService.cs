using Kviz.Migrations.Tables;
using Kviz.Model;

namespace Kviz.Services
{
    public interface IDataService
    {
        public Task<List<QuizTable>> GetQuizes();
        public Task<List<QuizTable>> GetQuizesByUserId(int userId);
        public void CreateNewQuiz(Quiz quiz, int UserId);
        public Task<Quiz> GetQuizByIdAsync(int Id);
        public Task<int> GetQuizUserIdByQuizIdAsync(int quizId);
        public void UpdateQuiz(Quiz quiz, int Id);
        public Task<int> CreateSession(int quizId, int userId);
        public Task<List<SessionTable>> GetSessions();
        public void SetSessionDate(int Id);
        public Task<SessionTable> GetSessionById(int Id);
        public Task<List<SessionTable>> GetSessionsByUserId(int userId);
        public void SaveToHistory(Dictionary<Answer, List<string>> userAnswers, int questionId, int sessionId);
    }
}

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
        public Task SaveToHistory(int quizHistoryId, Question question, Dictionary<string, Answer> submittedAnswers);
        public Task<int> InitHistory(string quizName, int sessionId, int userId);
        public Task<List<HistoryTable>> GetHistoriesByUserIdAsync(int userId);
        public Task<QuizHistoryTable> GetNameOfQuiz(int historyId);
        public Task DeleteHistoryByIdAsync(int historyId);
        public Task<QuizHistory> GetQuizHistoryDetailAync(int historyId);
    }
}

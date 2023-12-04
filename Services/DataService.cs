using Kviz.Data;
using Kviz.Migrations.Tables;
using Kviz.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Kviz.Services
{

  public class DataService : IDataService
  {

    private DataContext _context;
    public int SessionId { get; set; }

    public DataService(DataContext context)
    {

      _context = context;
    }

    public async Task<List<QuizTable>> GetQuizes()
    {
      return await _context.Quizes.Where(q => q.User_Id == 1).ToListAsync();
      //TODO make it so the param is a User.Id
    }

    public async Task<List<QuizTable>> GetQuizesByUserId(int userId)
    {
      return await _context.Quizes.Where(q => q.User_Id == userId).ToListAsync();
    }

    public async void CreateNewQuiz(Quiz quiz, int userId)
    {
      QuizTable quizTable = new QuizTable
      {
        Name = quiz.Name,
        User_Id = 1

      };
      await _context.Quizes.AddAsync(quizTable);
      await _context.SaveChangesAsync();
      SaveQuestionsAndAnswers(quizTable.Id, quiz);

    }

    public async Task<int> GetQuizUserIdByQuizIdAsync(int quizId)
    {
      var quiz = await _context.Quizes.Where(q => q.Id == quizId).SingleAsync();
      return quiz.User_Id;
    }

    public async Task<Quiz> GetQuizByIdAsync(int Id)
    {
      try
      {
        QuizTable quizTable = await _context.Quizes.Where(q => q.Id == Id).SingleAsync();
        Quiz quiz = new Quiz(quizTable);
        List<QuestionTable> questionTables = await _context.Questions.Where(q => q.Quiz_Id == quizTable.Id).ToListAsync();
        foreach (QuestionTable questionTable in questionTables)
        {
          Question question = new Question(questionTable);
          List<AnswerTable> answerTables = await _context.Answers.Where(q => q.Question_Id == questionTable.Id).ToListAsync();
          foreach (AnswerTable answerTable in answerTables)
          {
            question.Answers.Add(new Answer(answerTable));
          }
          quiz.Questions.Add(question);
        }
        return quiz;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        throw new Exception(ex.Message);
      }
    }

    public async void UpdateQuiz(Quiz quiz, int Id)
    {
      var quizRow = _context.Quizes.Find(Id);
      quizRow.Name = quiz.Name;
      var questionQuery = _context.Questions.Where(i => i.Quiz_Id == Id).ToList();
      _context.Questions.RemoveRange(questionQuery);
      await _context.SaveChangesAsync();
      SaveQuestionsAndAnswers(Id, quiz);
    }

    public async void SaveQuestionsAndAnswers(int quiz_Id, Quiz quiz)
    {
      foreach (Question q in quiz.Questions)
      {
        if (q.Text != null && q.Text != string.Empty && q.Answers.Count > 1)
        {
          QuestionTable qt = new QuestionTable()
          {
            Text = q.Text,
            Type = q.Type.ToString(),
            Quiz_Id = quiz_Id,
            Time = q.Time
          };
          await _context.Questions.AddAsync(qt);
          await _context.SaveChangesAsync();

          foreach (Answer a in q.Answers)
          {
            if (q.Text != null && q.Text != string.Empty)
            {
              AnswerTable at = new AnswerTable()
              {
                Text = a.Text,
                Correct = a.Correct,
                Question_Id = qt.Id
              };
              await _context.Answers.AddAsync(at);
            }
          }
        }
      }
      await _context.SaveChangesAsync();

    }

    public async Task<int> CreateSession(int quizId, int userId)
    {
      SessionTable newSession = new SessionTable();
      newSession.Quiz_Id = quizId;
      newSession.User_Id = userId;
      await _context.Sessions.AddAsync(newSession);
      await _context.SaveChangesAsync();
      return newSession.Id;

    }

    public async Task<List<SessionTable>> GetSessions()
    {
      return await _context.Sessions.ToListAsync();
    }

    public async Task<SessionTable> GetSessionById(int Id)
    {
      return await _context.Sessions.Where(s => s.Id == Id).SingleAsync();

    }

    public async Task<List<SessionTable>> GetSessionsByUserId(int userId)
    {
      return await _context.Sessions.Where(u => u.User_Id == userId).ToListAsync();
    }

    public async void SetSessionDate(int Id)
    {
      var session = await _context.Sessions.Where(s => s.Id == Id).SingleAsync();
      session.Date = DateTime.Now;
    }

    public async Task SaveToHistory(int quizHistoryId, Question question, ConcurrentDictionary<string, Answer> submittedAnswers)
    {
      QuestionHistoryTable questionHistoryTable = new QuestionHistoryTable()
      {
        Text = question.Text,
        QuizHistory_Id = quizHistoryId,
      };

      await _context.QuestionHistories.AddAsync(questionHistoryTable);
      await _context.SaveChangesAsync();
      List<AnswerHistoryTable> answerHistories = new List<AnswerHistoryTable>();
      foreach (KeyValuePair<string, Answer> entry in submittedAnswers)
      {
        answerHistories.Add(new AnswerHistoryTable()
        {
          PlayerName = entry.Key,
          Correct = entry.Value.Correct,
          Text = entry.Value.Text,
          QuestionHistory_Id = questionHistoryTable.Id
        });
      }
      await _context.AnswerHistories.AddRangeAsync(answerHistories);
      await _context.SaveChangesAsync();
    }

    public async Task<int> InitHistory(string quizName, int sessionId, int userId)
    {
      HistoryTable hTable = new HistoryTable(userId, DateTime.Now, sessionId);
      await _context.Histories.AddAsync(hTable);
      await _context.SaveChangesAsync();

      QuizHistoryTable quizHistoryTable = new QuizHistoryTable()
      {
        Name = quizName,
        History_Id = hTable.Id
      };
      await _context.QuizHistories.AddAsync(quizHistoryTable);
      await _context.SaveChangesAsync();
      return quizHistoryTable.Id;
    }

    public async Task<List<HistoryTable>> GetHistoriesByUserIdAsync(int userId)
    {
      return await _context.Histories.Where(h => h.User_Id == userId).ToListAsync();
    }

    public async Task<QuizHistoryTable> GetNameOfQuiz(int historyId)
    {
      return await _context.QuizHistories.Where(h => h.History_Id == historyId).SingleAsync();
    }

    public async Task DeleteHistoryByIdAsync(int historyId)
    {
      var history = await _context.Histories.Where(h => historyId == h.Id).SingleAsync();
      _context.Histories.Remove(history!);
      await _context.SaveChangesAsync();
    }

    public async Task<QuizHistory> GetQuizHistoryDetailAync(int historyId)
    {
      QuizHistory quizHistory = new QuizHistory();
      var quizTable = await _context.QuizHistories.Where(i => i.History_Id == historyId).SingleAsync();
      quizHistory.Name = quizTable.Name;
      var questions = await _context.QuestionHistories.Where(q => q.QuizHistory_Id == quizTable.Id).ToListAsync();
      foreach (var question in questions)
      {
        QuestionHistory questionHistory = new QuestionHistory();
        questionHistory.Text = question.Text;
        List<AnswerHistoryTable> answerTableList = await _context.AnswerHistories.Where(a => a.QuestionHistory_Id == question.Id).ToListAsync();
        foreach (var answer in answerTableList)
        {
          AnswerHistory answerHistory = new AnswerHistory()
          {
            Text = answer.Text,
            PlayerName = answer.PlayerName,
            Correct = answer.Correct
          };
          questionHistory.answerHistories.Add(answerHistory);

        }
        quizHistory.questionHistories.Add(questionHistory);

      }
      return quizHistory;
    }
  }
}

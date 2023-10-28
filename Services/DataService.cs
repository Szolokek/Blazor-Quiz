﻿using Kviz.Data;
using Kviz.Migrations.Tables;
using Kviz.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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
        }

        public async void CreateNewQuiz(Quiz quiz)
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

        public async Task<Quiz> GetQuizByIdAsync(int Id)
        {
            try {
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
            }catch (Exception ex)
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
                if(q.Text != null && q.Text != string.Empty && q.Answers.Count > 1)
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

        public async Task<int> CreateSession(int Id)
        {
            SessionTable newSession = new SessionTable();
            newSession.Quiz_Id = Id;
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

        public async void SetSessionDate(int Id)
        {
            var session = await _context.Sessions.Where(s => s.Id == Id).SingleAsync();
            session.Date = DateTime.Now;
        }

    }
}

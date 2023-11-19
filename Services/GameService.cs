using Kviz.Data;
using Kviz.Migrations.Tables;
using Kviz.Migrations;
using Kviz.Model;
using System.Reflection.Metadata;
using System.Timers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Kviz.Services
{
    public class GameService
    {
        private int sessionId;
        public Quiz quiz;
        public int questionIndex = 0;
        private bool startGame;
        private bool closed;
        public bool leaderBoardView = false;
        public bool GameOver = false;
        public bool StartGame
        {
            get
            {
                return startGame;
            }
            set
            {
                startGame = value;
                StartGameEvent?.Invoke();
            }
        }

        public bool Closed
        {
            get { return closed; }
            set
            {
                closed = value;
                QuestionClosedEvent?.Invoke();
            }
        }

        public int Time { get; set; }
        public string TimeAsString
        {
            get => Time.ToString();
            set
            {
                if (int.TryParse(value, out var parsedValue))
                {
                    Time = parsedValue;
                }
            }
        }
        System.Timers.Timer QuestionTimer;
        System.Timers.Timer LeaderBoardTimer;

        public Dictionary<string, int> userPoints;
        public Dictionary<string, int> UserPoints { get; set; }
        private Dictionary<string, Answer> submittedAnswers;

        private List<string> users;
        private Dictionary<Answer, List<string>> userAnswers;
        public Dictionary<Answer, List<string>> UserAnswers { get; set; }
        private List<string> didntAnswer;


        public void AddToUserAnswers(Answer key, string value)
        {
            userAnswers[key].Add(value);
        }

        public void AddToSubmittedAnswers(Answer key, string nickname)
        {
            submittedAnswers.Add(nickname, key);
            UpdateEvent?.Invoke();
        }


        public event Func<Task> UpdateEvent;
        public event Func<Task> NextQuestionEvent;
        public event Func<Task> RevealAnswerEvent;
        public event Func<Task> StartGameEvent;
        public event Func<Task> TimerCountEvent;
        public event Func<Task> QuestionClosedEvent;
        public event Func<Task> TimesUpEvent;
        public event Func<Task> StateChangedEvent;
        public event Func<Task> LeaderboardEvent;


        public Question GetCurrentQuestion()
        {
            return quiz.Questions[questionIndex];
        }
        public List<Answer> GetCurrentAnswers()
        {
            return quiz.Questions[questionIndex].Answers;
        }

        private void SaveAnswersToDB()
        {
            //TODO implement the new model with didntAnswer and make new tables for history
            _dataService.SaveToHistory(userAnswers, quiz.Questions[questionIndex].Id, sessionId);
        }

        public void NextQuestion()
        {
            //SaveAnswersToDB();
            questionIndex++;
            if(questionIndex == quiz.Questions.Count)
            {
                GameOver = true;
            }
            else
            {
                userAnswers.Clear();
                submittedAnswers.Clear();
                LoadNextQuestion();
                Closed = false;
                NextQuestionEvent?.Invoke();
                QuestionTimer.Start();
            }
        }

        public void CloseQuestion()
        {
            if (true)
            {
                leaderBoardView = true;
                LeaderBoardTimer.Start();
                LeaderboardEvent?.Invoke();
                
            }
            else
            {
                NextQuestion();
            }
            
        }

        public void LoadNextQuestion()
        {
            userAnswers = new Dictionary<Answer, List<string>>();
            this.Time = quiz.Questions[questionIndex].Time;
            foreach (Answer a in quiz.Questions[questionIndex].Answers)
            {
                userAnswers.Add(a, new List<string>());
            }
        }

        public async void StartGameFirstQuestion(Quiz quiz)
        {
            //SessionTable sessionTable = await _dataService.GetSessionById(sessionId);
            //quiz = await _dataService.GetQuizByIdAsync(sessionTable.Quiz_Id);
            this.quiz = quiz;
            LoadNextQuestion();
            QuestionTimer.Start();
        }



        public void RevealAnswer()
        {
            Closed = true;
            QuestionTimer.Stop();

            //foreach (KeyValuePair<Answer, List<string>> entry in userAnswers)
            //{
            //    if (entry.Key.Correct)
            //    {
            //        foreach (string nickname in entry.Value)
            //        {
            //            userPoints[nickname] += 1;
            //        }
            //    }
            //}

            foreach(KeyValuePair<string, int> entry in userPoints)
            {
                if (submittedAnswers.ContainsKey(entry.Key))
                {
                    if (submittedAnswers[entry.Key].Correct)
                    {
                        userPoints[entry.Key] += 1;
                    }
                }
                else 
                {
                    didntAnswer.Add(entry.Key);
                }

            }


            RevealAnswerEvent?.Invoke();
        }
        private readonly IDataService _dataService;
        public GameService(IDataService dataService)
        {
            _dataService = dataService;
            InitializeAll();
        }

        public void InitializeAll()
        {
            StartGame = false;
            users = new List<string>();
            userPoints = new Dictionary<string, int>();
            submittedAnswers = new Dictionary<string, Answer>();
            QuestionTimer = new System.Timers.Timer();
            QuestionTimer.Interval = 1000;
            QuestionTimer.Elapsed += QuestionTimerOnElapsed;
            LeaderBoardTimer = new System.Timers.Timer();
            LeaderBoardTimer.Interval = 1000;
            LeaderBoardTimer.Elapsed += LeaderBoardTimerOnElapsed;
        }

        private void QuestionTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Time--;
            if (Time < 0)
            {
                RevealAnswer();
            }
            else
            {
                TimerCountEvent?.Invoke();
            }
        }

        public int LeaderBoardUpTime = 5;
        public string LeaderBoardUpTimeAsString
        {
            get => LeaderBoardUpTime.ToString();
            set
            {
                if (int.TryParse(value, out var parsedValue))
                {
                    LeaderBoardUpTime = parsedValue;
                }
            }
        }
        private void LeaderBoardTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            LeaderBoardUpTime--;
            if (LeaderBoardUpTime < 0)
            {
                LeaderBoardTimer.Stop();
                LeaderBoardUpTime = 5;
                leaderBoardView = false;
                NextQuestion();
            }
            else
            {
                LeaderboardEvent?.Invoke();
            }
        }

        public bool CheckIfUserAnsweredCorrectly(string nickname)
        {
            //foreach (Answer answer in GetCurrentQuestion().GetCorrectAnswers())
            //{
            //    if (userAnswers[answer].Contains(nickname))
            //    {
            //        return true;
            //    }
            //}
            //return false;
            if (submittedAnswers.ContainsKey(nickname))
            {
                if (submittedAnswers[nickname].Correct)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfUserAlreadyAnswered(string nickname)
        {
            if (submittedAnswers.ContainsKey(nickname))
            {
                return true;
            }
            return false;
        }

        public bool CheckIfNewUser(string nickname)
        {
            if (users.Contains(nickname))
            {
                return false;
            }
            return true;
        }

        public void AddUser(string nickname)
        {
            users.Add(nickname);
        }

        public void RemoveUser(string nickname)
        {
            users.Remove(nickname);
        }

        public bool CheckIfFirstJoin(string nickname)
        {
            if (userPoints.ContainsKey(nickname))
            {
                return false;
            }
            return true;
        }



    }
    
}

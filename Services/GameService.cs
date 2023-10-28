using Kviz.Data;
using Kviz.Model;
using System.Reflection.Metadata;
using System.Timers;

namespace Kviz.Services
{
    public class GameService
    {
        private bool startGame;
        private bool closed;
        public bool StartGame { 
            get 
            { 
                return startGame; 
            } 
            set 
            { 
                startGame = value;
                StartGameEvent?.Invoke();
            } }

        public bool Closed
        {
            get { return closed; }
            set 
            { 
                closed = value;
                QuestionClosedEvent?.Invoke();
            }
        }

        public string CurrentQuestion { get; set; }
        public List<string> CurrentAnswers { get; set; }
        public List<string> CorrectAnswers { get; set; }
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
        System.Timers.Timer timer;

        public Dictionary<string, int> UserPoints;

        public Dictionary<string, bool> Users { get; set; }
        public Dictionary<string, List<string>> UserAnswers { get; set; }

        public void AddToUserAnswers(string key, string value)
        {
            UserAnswers[key].Add(value);
            UpdateEvent?.Invoke();
        }


        public event Func<Task> UpdateEvent;
        public event Func<Task> NextQuestionEvent;
        public event Func<Task> RevealAnswerEvent;
        public event Func<Task> StartGameEvent;
        public event Func<Task> TimerCountEvent;
        public event Func<Task> QuestionClosedEvent;
        public event Func<Task> TimesUpEvent;

        public void NextQuestion(string question, List<string> answers, int time)
        {
            UserAnswers = new Dictionary<string, List<string>>();
            CurrentQuestion = question;
            CurrentAnswers = answers;
            this.Time = time;
            foreach(string a in answers)
            {
                UserAnswers.Add(a, new List<string>());
            }

            //Save answers to DB

            NextQuestionEvent?.Invoke();
            timer.Start();
        }

        public void StartGameFirstQuestion(string question, List<string> answers, int time)
        {
            UserAnswers = new Dictionary<string, List<string>>();
            CurrentQuestion = question;
            CurrentAnswers = answers;
            this.Time = time;
            foreach (string a in answers)
            {
                UserAnswers.Add(a, new List<string>());
            }
            NextQuestionEvent?.Invoke();
            timer.Start();
        }

        public void RevealAnswer(List<string> correctAnswers)
        {
            CorrectAnswers = correctAnswers;
            RevealAnswerEvent?.Invoke();
            timer.Stop();
        }

        public GameService()
        {
            StartGame = false;
            Users = new Dictionary<string, bool>();
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Time--;
            if(Time < 0)
            {
                TimesUpEvent?.Invoke();
            }
            else
            {
                TimerCountEvent?.Invoke();
            }
        }
    }
}

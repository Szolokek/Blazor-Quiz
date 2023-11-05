using Kviz.Migrations.Tables;
namespace Kviz.Model
{
    public class Question
    {
        public int Id { get; set; }
        public List<Answer> Answers { get; set; }
        public string? Text { get; set; }
        public int Time { get; set; }
        public QuestionType Type { get; set; }
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

        public Question()
        {
            Answers = new List<Answer>
            {
                new Answer()
            };
        }

        public Question(QuestionTable q)
        {
            Id = q.Id;
            Text = q.Text;
            Time = q.Time;
            Type = Enum.Parse<QuestionType>(q.Type);
            Answers = new List<Answer>();
        }

        public List<string> GetAnswersAsString()
        {
            List<string> list = new List<string>();
            foreach (var answer in Answers)
            {
                list.Add(answer.Text);
            }
            return list;
        }

        public List<Answer> GetCorrectAnswers()
        {
            List<Answer> list = new List<Answer>();
            foreach (var answer in Answers)
            {
                if (answer.Correct)
                {
                    list.Add(answer);
                }
            }
            return list;
        }

        public List<string> GetCorrectAnswersAsString()
        {
            List<string> list = new List<string>();
            List<Answer> correct = GetCorrectAnswers();
            foreach (var answer in correct)
            {
                list.Add(answer.Text);
            }
            return list;
        }

        public bool CheckIfAnswerIsCorrect(string answer)
        {
            return GetAnswersAsString().Contains(answer);
        }
    }
}

using Kviz.Migrations.Tables;
namespace Kviz.Model
{
    public class Quiz
    {
        public List<Question> Questions { get; set; }
        public string Name { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }

        public Quiz(QuizTable q)
        {
            Name = q.Name;
            Questions = new List<Question>();
        }
    }
}

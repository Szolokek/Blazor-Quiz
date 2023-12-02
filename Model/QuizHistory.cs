namespace Kviz.Model
{
    public class QuizHistory
    {
        public string Name { get; set; }
        public List<QuestionHistory> questionHistories { get; set; }

        public QuizHistory() 
        { 
            questionHistories = new List<QuestionHistory>();
        }
    }
}

namespace Kviz.Model
{
    public class QuestionHistory
    {
        public string Text { get; set; }
        public List<AnswerHistory> answerHistories { get; set; }

        public QuestionHistory() 
        { 
            answerHistories = new List<AnswerHistory>();
        }
    }
}

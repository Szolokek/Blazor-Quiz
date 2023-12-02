namespace Kviz.Migrations.Tables
{
    public class QuestionHistoryTable
    {
        public int Id { get; set; }
        public string Text{ get; set; }
        public int QuizHistory_Id { get; set; }
    }
}

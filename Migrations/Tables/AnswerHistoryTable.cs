using System.Reflection.Metadata.Ecma335;

namespace Kviz.Migrations.Tables
{
    public class AnswerHistoryTable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string PlayerName { get; set; }
        public bool Correct { get; set; }
        public int QuestionHistory_Id { get; set; }
    }
}

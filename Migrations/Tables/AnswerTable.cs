namespace Kviz.Migrations.Tables
{
    public class AnswerTable
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }
        public int Question_Id { get; set; }
    }
}

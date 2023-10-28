namespace Kviz.Migrations.Tables
{
    public class QuestionTable
    {


        public int Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public int Quiz_Id { get; set; }
        public int Time { get; set; }

    }
}

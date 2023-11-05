namespace Kviz.Migrations.Tables
{
    public class HistoryTable
    {
        public int Id { get; set; }
        public int Session_Id { get; set; }
        public string Nickname { get; set; }
        public int Question_Id { get; set; }
        public int Answer_Id { get; set; }

        public HistoryTable() { }

        public HistoryTable(int session_Id, string nickname, int question_Id, int answer_Id)
        {
            Session_Id = session_Id;
            Nickname = nickname;
            Question_Id = question_Id;
            Answer_Id = answer_Id;
        }
    }
}

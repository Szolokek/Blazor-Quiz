namespace Kviz.Migrations.Tables
{
    public class HistoryTable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int User_Id { get; set; }
        public int Session_Id { get; set; }

        public HistoryTable() { }

      public HistoryTable(int user_id, DateTime date, int sessionNumber) 
        { 
            User_Id = user_id;
            Date = date;
            Session_Id = sessionNumber;
        }
    }
}

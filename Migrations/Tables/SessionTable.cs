namespace Kviz.Migrations.Tables
{
    public class SessionTable
    {
        public int Id { get; set; }
        public int Quiz_Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Date { get; set; }
    }
}

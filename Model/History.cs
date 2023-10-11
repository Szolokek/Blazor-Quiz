namespace Kviz.Model
{
    public class History
    {
        public int Id { get; set; }
        public int Session_Id { get; set; }
        public string Nickname { get; set; }
        public int Question_Id { get; set; }
        public int Answer_Id { get; set; }

    }
}

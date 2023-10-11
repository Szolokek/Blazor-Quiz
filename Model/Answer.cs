namespace Kviz.Model
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }
        public int Quetstion_Id { get; set; }
    }
}

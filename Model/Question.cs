namespace Kviz.Model
{
    public class Question
    {


        public int Id { get; set; }
        public string Text { get; set; }
        public int Quiz_Id { get; set; }
        public string Type { get; set; }
    }
}

using Kviz.Migrations.Tables;
namespace Kviz.Model
{
    public class Answer
    {
        public int Id { get; set; }
        public string? Text { get; set;}
        public bool Correct { get; set;}
        public Answer()
        {

        }

        public Answer(AnswerTable a)
        {
            Id = a.Id;
            Text = a.Text;
            Correct = a.Correct;
        }

        public Answer(string text, bool correct)
        {
            Text = text;
            Correct = correct;
        }
    }

    
}

using Kviz.Migrations.Tables;
namespace Kviz.Model
{
    public class Answer
    {
        public string? Text { get; set;}
        public bool Correct { get; set;}
        public Answer()
        {

        }

        public Answer(AnswerTable a)
        {
            Text = a.Text;
            Correct = a.Correct;
        }
    }

    
}

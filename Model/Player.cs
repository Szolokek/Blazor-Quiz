namespace Kviz.Model
{
    public class Player
    {
        public string Name { get; set; }
        public bool AlreadyAnswered { get; set; }
        public Player(string name) 
        {
            Name = name;
        }
    }
}

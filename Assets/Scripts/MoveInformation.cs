namespace Assets.Scripts
{
    public class MoveInformation
    {
        public Cell From { get; }

        public Cell To { get; }

        public double benefit;

        public MoveInformation(Cell from, Cell to)
        {
            From = from;
            To = to;
        }
    }
}
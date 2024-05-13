namespace BountySystem
{
    public class Bounty
    {
        public string Name { get; private set; }
        public string Crime { get; private set; }
        public int Amount { get; private set; }

        public Bounty(string name, string crime, int amount)
        {
            Name = name;
            Crime = crime;
            Amount = amount;
        }
    }
}
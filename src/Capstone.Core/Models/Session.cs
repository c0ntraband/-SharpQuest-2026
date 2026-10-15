namespace Capstone.Core.Models
{
    public class Session
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string PlayerName { get; }
        public int StartingCredits { get; }
        public DateTime StartedAt { get; }

        public DateTime? EndedAt { get; private set; }

        public int Balance { get; private set; }

        public bool IsFinished => EndedAt is not null;

        public Session(string playerName, int startingCredits, DateTime startedAt)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(playerName);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(startingCredits);

            PlayerName = playerName;
            StartingCredits = startingCredits;
            StartedAt = startedAt;
            Balance = startingCredits;
        }

        public void PlaceBet(int bet)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bet);
            if (bet > Balance) throw new InvalidOperationException("The bet is above the balance.");

            Balance -= bet;
        }

        public void Collect(int payout)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(payout);

            Balance += payout;
        }

        public void End(DateTime endedAt)
        {
            if (IsFinished) throw new InvalidOperationException("This session has already ended.");
            if (endedAt < StartedAt) throw new ArgumentException("A session can't end before it started.", nameof(endedAt));

            EndedAt = endedAt;
        }
    }
}

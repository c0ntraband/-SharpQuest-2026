namespace Capstone.Core.Models
{
    public class Session
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string PlayerName { get; }
        public int StartingCredits { get; }
        public DateTime StartedAt { get; }

        public DateTime? EndedAt { get; private set; }

        public bool IsFinished => EndedAt is not null;

        public Session(string playerName, int startingCredits, DateTime startedAt)
        {
            if (string.IsNullOrWhiteSpace(playerName)) throw new ArgumentException("A session needs a player name.", nameof(playerName));
            if (startingCredits <= 0) throw new ArgumentException("Starting credits must be positive.", nameof(startingCredits));

            PlayerName = playerName;
            StartingCredits = startingCredits;
            StartedAt = startedAt;
        }

        public void End(DateTime endedAt)
        {
            if (IsFinished) throw new InvalidOperationException("This session has already ended.");
            if (endedAt < StartedAt) throw new ArgumentException("A session can't end before it started.", nameof(endedAt));

            EndedAt = endedAt;
        }
    }
}

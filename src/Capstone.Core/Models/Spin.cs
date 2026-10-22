using SharpQuest.Contracts;

namespace Capstone.Core.Models
{
    public abstract class Spin : IItem
    {
        public string Id { get; }
        public DateTime Time { get; }
        public int Bet { get; }
        public Reels Reels { get; }
        public int Payout { get; }
        public int CreditsAfter { get; }

        public abstract int Cost { get; }

        public bool IsWin => Payout > 0;

        public string DisplayName => $"{Reels} — bet {Bet}, won {Payout}";

        public abstract string Describe();

        protected Spin(string id, DateTime time, int bet, Reels reels, int payout, int creditsAfter)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(id);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bet);
            ArgumentOutOfRangeException.ThrowIfNegative(payout);
            ArgumentOutOfRangeException.ThrowIfNegative(creditsAfter);

            Id = id;
            Time = time;
            Bet = bet;
            Reels = reels;
            Payout = payout;
            CreditsAfter = creditsAfter;
        }

        public override string ToString() => $"{Id}: {DisplayName}";
    }
}

using SharpQuest.Contracts;

namespace Capstone.Core.Models
{
    public class Spin : IItem
    {
        public string Id { get; }
        public DateTime Time { get; }
        public int Bet { get; }
        public Reels Reels { get; }
        public int Payout { get; }
        public int CreditsAfter { get; }

        public bool IsWin => Payout > 0;

        public string DisplayName => $"{Reels} — bet {Bet}, won {Payout}";

        public Spin(string id, DateTime time, int bet, Reels reels, int payout, int creditsAfter)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("A spin needs an Id.", nameof(id));
            if (bet <= 0) throw new ArgumentException("The bet must be positive.", nameof(bet));
            if (payout < 0) throw new ArgumentException("The payout can't be negative.", nameof(payout));
            if (creditsAfter < 0) throw new ArgumentException("Credits can't go below zero.", nameof(creditsAfter));

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
